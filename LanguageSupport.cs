using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GlobalEnums;
using Language;
using LanguageSupport.Consts;
using Modding;
using UnityEngine;

namespace LanguageSupport
{
    class LanguageSupport : Mod
    {
        internal static LanguageSupport Instance;

        public LanguageStrings LangStrings { get; private set; }

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

            LangStrings = new LanguageStrings();

            InitCallbacks();

            InitLanguage();
        }

        public override void Initialize()
        {
            Log("Initializing");
            Instance = this;

            foreach (var t in GameObject.FindObjectsOfType<UnityEngine.UI.MenuLanguageSetting>())
            {
                t.RefreshControls();
            }

            Log("Initialized");
        }

        private void InitCallbacks()
        {
            // Hooks
            ModHooks.Instance.LanguageGetHook += OnLanguageGetHook;
            On.UnityEngine.UI.MenuLanguageSetting.RefreshAvailableLanguages += OnMenuLanguageSettingRefreshAvailableLanguages;
            On.Language.Language.HasLanguageFile += OnLanguageHasLanguageFile;
            On.Language.Language.LoadAvailableLanguages += OnLanguageLoadAvailableLanguages;
            On.LocalizationSettings.GetLanguageEnum += OnLocalizationSettingsGetLanguageEnum;
        }

        private void InitLanguage()
        {
            typeof(Language.Language).GetMethod("LoadAvailableLanguages", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            Language.Language.LoadLanguage();
        }

        private void OnMenuLanguageSettingRefreshAvailableLanguages(On.UnityEngine.UI.MenuLanguageSetting.orig_RefreshAvailableLanguages orig, UnityEngine.UI.MenuLanguageSetting self)
        {
            orig(self);
            List<SupportedLanguages> supLang = new List<SupportedLanguages>((SupportedLanguages[]) self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
            foreach (var pair in LangStrings.jsonDict)
            {
                try
                {
                    SupportedLanguages t = (SupportedLanguages) Enum.Parse(typeof(LanguageCode), pair.Key, true);
                    supLang.Add(t);
                }
                catch (Exception e)
                {
                    supLang.Add((SupportedLanguages) LangStrings.numberToName.First(x => x.Value == pair.Key).Key);
                }
            }
            self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, supLang.ToArray());
        }

        private bool OnLanguageHasLanguageFile(On.Language.Language.orig_HasLanguageFile orig, string lang, string sheetTitle)
        {
            var ret = orig(lang, sheetTitle);
            if (!ret)
            {
                foreach (var pair in LangStrings.jsonDict)
                {
                    if (lang == pair.Key)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        private void OnLanguageLoadAvailableLanguages(On.Language.Language.orig_LoadAvailableLanguages orig)
        {
            orig();
            List<string> langs = (List<string>) typeof(Language.Language).GetField("availableLanguages", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            foreach (var pair in LangStrings.jsonDict)
            {
                langs.Add(pair.Key);
            }
            Log("Discovered Languages:");
            foreach (var s in langs)
            {
                Log($"Language: \"{s}\"");
            }

            typeof(Language.Language).GetField("availableLanguages", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, langs);
        }

        private LanguageCode OnLocalizationSettingsGetLanguageEnum(On.LocalizationSettings.orig_GetLanguageEnum orig, string langCode)
        {
            try
            {
                Enum.Parse(typeof(LanguageCode), langCode, true);
                return orig(langCode);
            }
            catch (Exception e)
            {
                foreach (var pair in LangStrings.numberToName)
                {
                    if (langCode == pair.Key.ToString())
                    {
                        return (LanguageCode) pair.Key;
                    }
                }
            }
            return LanguageCode.EN;
        }

        #region Get/Set Hooks

        private string OnLanguageGetHook(string key, string sheet)
        {
            if (sheet == "MainMenu")
            {
                foreach (var pair in LangStrings.numberToName)
                {
                    if (key == $"LANG_{pair.Key}")
                    {
                        return pair.Value;
                    }
                }
            }
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return Language.Language.GetInternal(key, sheet);
        }

        #endregion Get/Set Hooks

    }
}
