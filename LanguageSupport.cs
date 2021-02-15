using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GlobalEnums;
using Language;
using Modding;
using On.UnityEngine.UI;
using UnityEngine;

namespace LanguageSupport
{
    class LanguageSupport : Mod
    {
        internal static LanguageSupport Instance;

        private readonly string FOLDER = "LanguageSupport";
        private readonly string DIR;

        public override string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string ver = asm.GetName().Version.ToString();
            SHA1 sha1 = SHA1.Create();
            FileStream stream = File.OpenRead(asm.Location);
            byte[] hashBytes = sha1.ComputeHash(stream);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            stream.Close();
            sha1.Clear();
            return $"{ver}-{hash.Substring(0, 6)}";
        }

        public LanguageSupport() : base("Language Support")
        {
            Instance = this;

            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.MacOSX:
                    DIR = Path.GetFullPath(Application.dataPath + "/Resources/Data/Managed/Mods/" + FOLDER);
                    break;
                default:
                    DIR = Path.GetFullPath(Application.dataPath + "/Managed/Mods/" + FOLDER);
                    break;
            }
            if (!Directory.Exists(DIR))
            {
                Directory.CreateDirectory(DIR);
            }
        }

        public override void Initialize()
        {
            Log("Initializing");
            Instance = this;

            InitCallbacks();

            InitLanguage();

            foreach (var t in GameObject.FindObjectsOfType<UnityEngine.UI.MenuLanguageSetting>())
            {
                t.RefreshControls();
            }

            Log("Initialized");
        }

        private void InitCallbacks()
        {
            // Hooks
            On.Language.Language.HasLanguageFile += OnLanguageHasLanguageFile;
            On.Language.Language.GetLanguageFileContents += OnLanguageGetLanguageFileContents;
            On.UnityEngine.UI.MenuLanguageSetting.RefreshAvailableLanguages += OnMenuLanguageSettingRefreshAvailableLanguages;
        }

        private void InitLanguage()
        {
            #region Dump English Files

            if (!Directory.Exists($"{DIR}/EN"))
            {
                Directory.CreateDirectory($"{DIR}/EN");
            }
            string[] sheets = new string[]
            {
                "Achievements",
                "Backer Messages",
                "Banker",
                "Charm Slug",
                "Cornifer",
                "CP2",
                "CP3",
                "Credits List",
                "Dream Witch",
                "Dreamers",
                "Elderbug",
                "Enemy Dreams",
                "General",
                "Ghosts",
                "Hornet",
                "Hunter",
                "Iselda",
                "Jiji",
                "Journal",
                "Lore Tablets",
                "MainMenu",
                "Map Zones",
                "Minor NPC",
                "Nailmasters",
                "Nailsmith",
                "Prices",
                "Prompts",
                "Quirrel",
                "Relic Dealer",
                "Shaman",
                "Sly",
                "Stag",
                "StagMenu",
                "Titles",
                "UI",
                "Zote"
            };
            foreach (var sheet in sheets)
            {
                if (!File.Exists($"{DIR}/EN/{sheet}.txt"))
                {
                    string t = ((TextAsset)Resources.Load($"Languages/EN_{sheet}", typeof(TextAsset))).text;
                    using (StreamWriter outputFile = new StreamWriter($"{DIR}/EN/{sheet}.txt"))
                    {
                        outputFile.Write(t);
                    }
                }
            }

            #endregion

            typeof(Language.Language).GetMethod("LoadAvailableLanguages", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            Language.Language.LoadLanguage();
        }

        private bool OnLanguageHasLanguageFile(On.Language.Language.orig_HasLanguageFile orig, string lang, string sheetTitle)
        {
            var ret = orig(lang, sheetTitle);
            if (!ret)
            {
                if (File.Exists($"{DIR}/{lang}/{sheetTitle}.txt"))
                {
                    ret = true;
                }
            }
            return ret;
        }

        private string OnLanguageGetLanguageFileContents(On.Language.Language.orig_GetLanguageFileContents orig, string sheetTitle)
        {
            string ret = orig(sheetTitle);
            if (ret == string.Empty)
            {
                if (File.Exists($"{DIR}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt"))
                {
                    return File.ReadAllText($"{DIR}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt");
                }
            }
            return ret;
        }

        private void OnMenuLanguageSettingRefreshAvailableLanguages(On.UnityEngine.UI.MenuLanguageSetting.orig_RefreshAvailableLanguages orig, UnityEngine.UI.MenuLanguageSetting self)
        {
            orig(self);
            SupportedLanguages[] langs;
            if (GameManager.instance.gameConfig.hideLanguageOption)
            {
                langs = (Enum.GetValues(typeof(TestingLanguages)) as SupportedLanguages[]);
            }
            else
            {
                langs = (Enum.GetValues(typeof(SupportedLanguages)) as SupportedLanguages[]);
            }

            List<SupportedLanguages> finalLangs = new List<SupportedLanguages>((SupportedLanguages[]) self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));

            foreach (var l in langs)
            {
                if (File.Exists($"{DIR}/{l}/General.txt"))
                {
                    finalLangs.Add(l);
                }
            }

            self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, finalLangs.ToArray());

            self.optionList = new string[finalLangs.Count];
            for (int i = 0; i < finalLangs.Count; i++)
            {
                self.optionList[i] = finalLangs[i].ToString();
            }
        }
    }
}
