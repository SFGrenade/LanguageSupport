using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using GlobalEnums;
using Language;
using Modding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFCore.Utils;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LanguageSupport
{
    internal class TmpFontConverter : CustomCreationConverter<TMP_FontAsset>
    {
        public override TMP_FontAsset Create(Type objectType)
        {
            return ScriptableObject.CreateInstance<TMP_FontAsset>();
        }
    }

    internal class LanguageSupport : Mod
    {
        internal static LanguageSupport Instance;
        private readonly string DIR;

        private readonly string FOLDER = "LanguageSupport";

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

            if (!Directory.Exists(DIR)) Directory.CreateDirectory(DIR);
        }

        public override string GetVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var ver = asm.GetName().Version.ToString();
            var sha1 = SHA1.Create();
            var stream = File.OpenRead(asm.Location);
            var hashBytes = sha1.ComputeHash(stream);
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            stream.Close();
            sha1.Clear();
            return $"{ver}-{hash.Substring(0, 6)}";
        }

        public override void Initialize()
        {
            Log("Initializing");
            Instance = this;

            InitCallbacks();

            InitLanguage();

            foreach (var t in Object.FindObjectsOfType<UnityEngine.UI.MenuLanguageSetting>()) t.RefreshControls();

            Log("Initialized");
        }

        private void InitCallbacks()
        {
            // Hooks
            On.Language.Language.HasLanguageFile += OnLanguageHasLanguageFile;
            On.Language.Language.GetLanguageFileContents += OnLanguageGetLanguageFileContents;
            On.UnityEngine.UI.MenuLanguageSetting.RefreshAvailableLanguages += OnMenuLanguageSettingRefreshAvailableLanguages;
            On.UnityEngine.UI.MenuLanguageSetting.PushUpdateOptionList += OnMenuLanguageSettingPushUpdateOptionList;
            On.ChangeFontByLanguage.SetFont += OnChangeFontByLanguageSetFont;

            On.TMPro.TextMeshPro.LoadFontAsset += OnTextMeshProLoadFontAsset;
            On.TMPro.TMP_FontAsset.ReadFontDefinition += OnTMP_FontAssetReadFontDefinition;
        }

        private void OnTMP_FontAssetReadFontDefinition(On.TMPro.TMP_FontAsset.orig_ReadFontDefinition orig, TMP_FontAsset self)
        {
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, FaceInfo>("m_fontInfo")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, Texture2D>("atlas")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, List<TMP_Glyph>>("m_glyphInfoList")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, Dictionary<int, KerningPair>>("m_characterDictionary")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, KerningTable>("m_kerningDictionary")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, List<TMP_FontAsset>>("m_kerningPair")}");
            Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, FontCreationSetting>("fontCreationSettings")}");

            orig(self);
        }

        private void OnTextMeshProLoadFontAsset(On.TMPro.TextMeshPro.orig_LoadFontAsset orig, TextMeshPro self)
        {
            var m_fontAsset = self.GetAttr<TextMeshPro, TMP_FontAsset>("m_fontAsset");
            var m_renderer = self.GetAttr<TextMeshPro, Renderer>("m_renderer");
            var m_sharedMaterial = self.GetAttr<TextMeshPro, Material>("m_sharedMaterial");

            if (m_fontAsset.name == "noto_serif_thai_bold_tmpro")
            {
                Log($"OnTextMeshProLoadFontAsset - {m_fontAsset.characterDictionary}"); // ""
                if (m_fontAsset.characterDictionary == null)
                {
                    m_fontAsset.ReadFontDefinition();
                }
                Log($"OnTextMeshProLoadFontAsset - {m_fontAsset.characterDictionary}"); // ""
                Log($"OnTextMeshProLoadFontAsset - {m_renderer.sharedMaterial}"); // "perpetua_tmpro Material (UnityEngine.Material)"
                Log($"OnTextMeshProLoadFontAsset - {m_renderer.sharedMaterial.mainTexture}"); // "perpetua_tmpro Atlas (UnityEngine.Texture2D)"
                Log($"OnTextMeshProLoadFontAsset - {m_fontAsset.atlas}"); // ""
                Log($"OnTextMeshProLoadFontAsset - {m_fontAsset.material}");
                Log($"OnTextMeshProLoadFontAsset - {m_renderer.receiveShadows}");
                Log($"OnTextMeshProLoadFontAsset - {m_renderer.shadowCastingMode}");
            }

            orig(self);
        }

        private static AssetBundle abFa = null;
        private static TMP_FontAsset fa = null;
        private static Sprite atlas = null;

        private void OnChangeFontByLanguageSetFont(On.ChangeFontByLanguage.orig_SetFont orig, ChangeFontByLanguage self)
        {
            orig(self);

            Log(1);

            if (Language.Language.CurrentLanguage() == LanguageCode.TH)
            {
                Log(2);

                bool json = true;

                if (json)
                {
                    if (fa == null)
                    {
                        Log(3);
                        Assembly asm = Assembly.GetExecutingAssembly();
                        Log(4);
                        using (Stream s = asm.GetManifestResourceStream("LanguageSupport.Resources.noto_serif_thai_bold.json"))
                        {
                            Log(5);
                            if (s == null) return;

                            Log(6);
                            byte[] buffer = new byte[s.Length];
                            Log(7);
                            s.Read(buffer, 0, buffer.Length);
                            Log(8);
                            s.Dispose();

                            Log(9);
                            string jsonText = System.Text.Encoding.UTF8.GetString(buffer);

                            Log(10);
                            fa = JsonConvert.DeserializeObject<TMP_FontAsset>(jsonText, new TmpFontConverter());
                            Log(10.5);
                            fa.name = "noto_serif_thai_bold_tmpro";
                            Log(11);
                            Object.DontDestroyOnLoad(fa);
                        }

                        fa.atlas = null; // texture2d
                        fa.material = new Material(Shader.Find("GUI/Text Shader"));

                        using (Stream s = asm.GetManifestResourceStream("LanguageSupport.Resources.noto_serif_thai_bold.png"))
                        {
                            if (s != null)
                            {
                                byte[] buffer = new byte[s.Length];
                                s.Read(buffer, 0, buffer.Length);
                                s.Dispose();

                                //Create texture from bytes
                                var tex = new Texture2D(2, 2);

                                tex.LoadImage(buffer, true);

                                // Create sprite from texture
                                // Split is to cut off the DreamKing.Resources. and the .png
                                atlas = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                                Object.DontDestroyOnLoad(atlas);
                            }
                        }
                        fa.material.SetTexture("_MainTex", atlas.texture);
                        fa.atlas = atlas.texture;

                        fa.material.SetColor("_Color", Color.white);
                    }

                    if (fa != null)
                    {
                        Log(20);
                        self.GetAttr<ChangeFontByLanguage, TextMeshPro>("tmpro").font = fa;
                        //var tmp = self.GetAttr<ChangeFontByLanguage, TextMeshPro>("tmpro");
                        //Log(21);
                        //tmp.font = fa;
                        //Log(22);
                        //self.SetAttr<ChangeFontByLanguage, TextMeshPro>("tmpro", tmp);
                    }
                }
                else
                {
                    if (abFa == null)
                    {
                        Log(12);
                        Assembly asm = Assembly.GetExecutingAssembly();
                        Log(13);
                        using (Stream s = asm.GetManifestResourceStream("LanguageSupport.Resources.tmprofont"))
                        {
                            Log(14);
                            if (s == null) return;

                            Log(15);
                            abFa = AssetBundle.LoadFromStream(s);
                            Log(16);
                            Object.DontDestroyOnLoad(abFa);
                        }
                    }

                    Log(17);

                    if (fa == null && abFa != null)
                    {
                        Log(18);
                        fa = abFa.LoadAsset<TMP_FontAsset>("noto_serif_thai_bold.asset");
                        Log(18.5);
                        fa.name = "noto_serif_thai_bold_tmpro";
                        Log(19);
                        Object.DontDestroyOnLoad(fa);
                    }

                    if (fa != null)
                    {
                        Log(20);
                        var tmp = (TextMeshPro) typeof(ChangeFontByLanguage).GetField("tmpro", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                        Log(21);
                        tmp.font = fa;
                        Log(22);
                        typeof(ChangeFontByLanguage).GetField("tmpro", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, tmp);
                    }
                }
            }
            Log(23);
        }

        private void Log(string message)
        {
            Modding.Logger.Log(message);
            Debug.Log($"[SFG Thing] - {message}");
        }
        private void Log(object message)
        {
            Log($"{message}");
        }

        private void InitLanguage()
        {
            #region Dump English Files

            if (!Directory.Exists($"{DIR}/EN")) Directory.CreateDirectory($"{DIR}/EN");
            string[] sheets =
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
                if (!File.Exists($"{DIR}/EN/{sheet}.txt"))
                {
                    var t = ((TextAsset) Resources.Load($"Languages/EN_{sheet}", typeof(TextAsset))).text;
                    using (var outputFile = new StreamWriter($"{DIR}/EN/{sheet}.txt"))
                    {
                        outputFile.Write(t);
                    }
                }

            #endregion

            Language.Language.LoadAvailableLanguages();
            Language.Language.LoadLanguage();
        }

        private void OnMenuLanguageSettingPushUpdateOptionList(On.UnityEngine.UI.MenuLanguageSetting.orig_PushUpdateOptionList orig, UnityEngine.UI.MenuLanguageSetting self)
        {
            orig(self);
            SupportedLanguages[] langs = (SupportedLanguages[]) self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
            string[] array = new string[langs.Length];
            for (int i = 0; i < langs.Length; i++)
            {
                array[i] = ((LanguageCode) langs[i]).ToString();
            }
            self.SetOptionList(array);
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
            LanguageCode[] customLangs;
            if (GameManager.instance.gameConfig.hideLanguageOption)
            {
                customLangs = (Enum.GetValues(typeof(LanguageCode)) as LanguageCode[]);
            }
            else
            {
                customLangs = (Enum.GetValues(typeof(LanguageCode)) as LanguageCode[]);
            }

            SupportedLanguages[] langs = (SupportedLanguages[]) self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
            List<SupportedLanguages> finalLangs = new List<SupportedLanguages>();

            foreach (var l in customLangs)
            {
                if (File.Exists($"{DIR}/{l}/General.txt") || (langs.Contains((SupportedLanguages) l)))
                {
                    finalLangs.Add((SupportedLanguages) l);
                }
            }

            self.GetType().GetField("langs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, finalLangs.ToArray());

            //self.optionList = new string[finalLangs.Count];
            //for (int i = 0; i < finalLangs.Count; i++)
            //{
            //    self.optionList[i] = finalLangs[i].ToString();
            //}
        }
    }
}