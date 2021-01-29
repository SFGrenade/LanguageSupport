using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GlobalEnums;
using LanguageSupport.Consts;
using Modding;
using On.UnityEngine.UI;
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
        }

        private void OnMenuLanguageSettingRefreshAvailableLanguages(MenuLanguageSetting.orig_RefreshAvailableLanguages orig, UnityEngine.UI.MenuLanguageSetting self)
        {
            orig(self);
            Log(1);
            List<SupportedLanguages> supLang = new List<SupportedLanguages>((SupportedLanguages[]) self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
            Log(2);
            var langNames = LangStrings.GetLanguages();
            Log(3);
            for (int i = 256; i < 256 + langNames.Count; i++)
            {
                Log(4);
                supLang.Add((SupportedLanguages) i);
            }
            Log(5);
            self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, supLang.ToArray());
            Log(6);
        }

        #region Get/Set Hooks

        private string OnLanguageGetHook(string key, string sheet)
        {
            if (sheet == "MainMenu")
            {
                foreach (var pair in LangStrings.GetLanguages())
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
