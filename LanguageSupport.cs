using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using GlobalEnums;
using JetBrains.Annotations;
using Language;
using Modding;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LanguageSupport;

[UsedImplicitly]
internal class LanguageSupport : Mod
{
    internal static LanguageSupport Instance;
    private readonly string _dir;

    private readonly string _folder = "LanguageSupport";

    public LanguageSupport() : base("Language Support")
    {
        Instance = this;

        _dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException("I have no idea how you did this, but good luck figuring it out."), _folder);

        if (!Directory.Exists(_dir))
        {
            Directory.CreateDirectory(_dir);
        }
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
        
        // ToDo these break text display
        //On.ChangeFontByLanguage.SetFont += OnChangeFontByLanguageSetFont;
        //On.TMPro.TextMeshPro.LoadFontAsset += OnTextMeshProLoadFontAsset;
        //On.TMPro.TMP_FontAsset.ReadFontDefinition += OnTMP_FontAssetReadFontDefinition;
    }

    private void OnTMP_FontAssetReadFontDefinition(On.TMPro.TMP_FontAsset.orig_ReadFontDefinition orig, TMP_FontAsset self)
    {
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, FaceInfo>("m_fontInfo")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, Texture2D>("atlas")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, List<TMP_Glyph>>("m_glyphInfoList")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, Dictionary<int, KerningPair>>("m_characterDictionary")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, KerningTable>("m_kerningDictionary")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, List<TMP_FontAsset>>("m_kerningPair")}");
        //Log($"OnTMP_FontAssetReadFontDefinition - {self.GetAttr<TMP_FontAsset, FontCreationSetting>("fontCreationSettings")}");

        orig(self);
    }

    private void OnTextMeshProLoadFontAsset(On.TMPro.TextMeshPro.orig_LoadFontAsset orig, TextMeshPro self)
    {
        var mFontAsset = ReflectionHelper.GetField<TextMeshPro, TMP_FontAsset>(self,"m_fontAsset");
        var mRenderer = ReflectionHelper.GetField<TextMeshPro, Renderer>(self,"m_renderer");
        var mSharedMaterial = ReflectionHelper.GetField<TextMeshPro, Material>(self,"m_sharedMaterial");

        if (mFontAsset.name == "noto_serif_thai_bold_tmpro")
        {
            //Log($"OnTextMeshProLoadFontAsset - {mFontAsset.characterDictionary}"); // ""
            if (mFontAsset.characterDictionary == null)
            {
                mFontAsset.ReadFontDefinition();
            }

            //Log($"OnTextMeshProLoadFontAsset - {mFontAsset.characterDictionary}"); // ""
            //Log($"OnTextMeshProLoadFontAsset - {mRenderer.sharedMaterial}"); // "perpetua_tmpro Material (UnityEngine.Material)"
            //Log($"OnTextMeshProLoadFontAsset - {mRenderer.sharedMaterial.mainTexture}"); // "perpetua_tmpro Atlas (UnityEngine.Texture2D)"
            //Log($"OnTextMeshProLoadFontAsset - {mFontAsset.atlas}"); // ""
            //Log($"OnTextMeshProLoadFontAsset - {mFontAsset.material}");
            //Log($"OnTextMeshProLoadFontAsset - {mRenderer.receiveShadows}");
            //Log($"OnTextMeshProLoadFontAsset - {mRenderer.shadowCastingMode}");
        }

        orig(self);
    }

    private static AssetBundle _abFa = null;
    private static TMP_FontAsset _fa = null;
    private static Sprite _atlas = null;

    private void OnChangeFontByLanguageSetFont(On.ChangeFontByLanguage.orig_SetFont orig, ChangeFontByLanguage self)
    {
        orig(self);
            
        var tmp = ReflectionHelper.GetField<ChangeFontByLanguage, TextMeshPro>(self, "tmpro");
        var tmpMat = _fa.material;
        tmpMat.shader = tmp.font.material.shader;
        tmpMat.SetTexture("_BumpMap",    null);
        tmpMat.SetTexture("_Cube",       null);
        tmpMat.SetTexture("_FaceTex",    null);
        tmpMat.SetTexture("_MainTex",    _fa.atlas);
        tmpMat.SetTexture("_OutlineTex", null);
        tmpMat.SetFloat("_Ambient", 0.5f);
        tmpMat.SetFloat("_Bevel", 0.5f);
        tmpMat.SetFloat("_BevelClamp", 0);
        tmpMat.SetFloat("_BevelOffset", 0);
        tmpMat.SetFloat("_BevelRoundness", 0);
        tmpMat.SetFloat("_BevelWidth", 0);
        tmpMat.SetFloat("_BumpFace", 0);
        tmpMat.SetFloat("_BumpOutline", 0);
        tmpMat.SetFloat("_ColorMask", 15);
        tmpMat.SetFloat("_Diffuse", 0.5f);
        tmpMat.SetFloat("_FaceDilate", 0);
        tmpMat.SetFloat("_FaceUVSpeedX", 0);
        tmpMat.SetFloat("_FaceUVSpeedY", 0);
        tmpMat.SetFloat("_GlowInner", 0.05f);
        tmpMat.SetFloat("_GlowOffset", 0);
        tmpMat.SetFloat("_GlowOuter", 0.05f);
        tmpMat.SetFloat("_GlowPower", 0.75f);
        tmpMat.SetFloat("_GradientScale", 5);
        tmpMat.SetFloat("_LightAngle", 3.1416f);
        tmpMat.SetFloat("_MaskID", 0);
        tmpMat.SetFloat("_MaskSoftnessX", 0);
        tmpMat.SetFloat("_MaskSoftnessY", 0);
        tmpMat.SetFloat("_OutlineSoftness", 0);
        tmpMat.SetFloat("_OutlineUVSpeedX", 0);
        tmpMat.SetFloat("_OutlineUVSpeedY", 0);
        tmpMat.SetFloat("_OutlineWidth", 0);
        tmpMat.SetFloat("_PerspectiveFilter", 0.875f);
        tmpMat.SetFloat("_Reflectivity", 10);
        tmpMat.SetFloat("_ScaleRatioA", 0.8f);
        tmpMat.SetFloat("_ScaleRatioB", 0.8f);
        tmpMat.SetFloat("_ScaleRatioC", 0.6504065f);
        tmpMat.SetFloat("_ScaleX", 1);
        tmpMat.SetFloat("_ScaleY", 1);
        tmpMat.SetFloat("_ShaderFlags", 0);
        tmpMat.SetFloat("_SpecularPower", 2);
        tmpMat.SetFloat("_Stencil", 0);
        tmpMat.SetFloat("_StencilComp", 8);
        tmpMat.SetFloat("_StencilOp", 0);
        tmpMat.SetFloat("_StencilReadMask", 255);
        tmpMat.SetFloat("_StencilWriteMask", 255);
        tmpMat.SetFloat("_TextureHeight", _fa.atlas.height);
        tmpMat.SetFloat("_TextureWidth", _fa.atlas.width);
        tmpMat.SetFloat("_UnderlayDilate", 0.23f);
        tmpMat.SetFloat("_UnderlayOffsetX", 0);
        tmpMat.SetFloat("_UnderlayOffsetY", 0);
        tmpMat.SetFloat("_UnderlaySoftness", 1);
        tmpMat.SetFloat("_VertexOffsetX", 0);
        tmpMat.SetFloat("_VertexOffsetY", 0);
        tmpMat.SetFloat("_WeightBold", 0.75f);
        tmpMat.SetFloat("_WeightNormal", 0);
        tmpMat.SetColor("_ClipRect", new Color(-10000, -10000, 10000, 10000));
        tmpMat.SetColor("_EnvMatrixRotation", new Color(0, 0, 0, 0));
        tmpMat.SetColor("_FaceColor", new Color(1, 1, 1, 1));
        tmpMat.SetColor("_GlowColor", new Color(0, 1, 0, 0.5f));
        tmpMat.SetColor("_MaskCoord", new Color(0, 0, 0, 0));
        tmpMat.SetColor("_OutlineCoord", new Color(0, 0, 0, 1));
        tmpMat.SetColor("_ReflectFaceColor", new Color(0, 0, 0, 1));
        tmpMat.SetColor("_ReflectOutlineColor", new Color(0, 0, 0, 1));
        tmpMat.SetColor("_SpecularColor", new Color(1, 1, 1, 1));
        tmpMat.SetColor("_UnderlayColor", new Color(0, 0, 0, 0.221f));
        _fa.material = tmpMat;
        tmp.font = _fa;
        //tmp.font.atlas = _fa.atlas;
        //Dictionary<int, TMP_Glyph> tmpGlyphs = _fa.characterDictionary;
        //foreach (var pair in tmpGlyphs)
        //{
        //    pair.Value.y = (_fa.atlas.height - (pair.Value.y + (pair.Value.height / 2))) - (pair.Value.height / 2);
        //}
        //tmp.font.SetAttr("m_characterDictionary", tmpGlyphs);
        //tmp.font.SetAttr("m_fontInfo", _fa.fontInfo);
        //tmp.font.SetAttr("m_kerningDictionary", _fa.kerningDictionary);
        //tmp.font.SetAttr("m_kerningInfo", _fa.kerningInfo);
        typeof(ChangeFontByLanguage).GetField("tmpro", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, tmp);
    }

    private new void Log(string message)
    {
        Modding.Logger.Log(message);
        Debug.Log($"[SFG Thing] - {message}");
    }

    private new void Log(object message)
    {
        Log($"{message}");
    }

    private void InitLanguage()
    {
        #region Dump All Files

        var langs = Enum.GetNames(typeof(SupportedLanguages));
        foreach (var lang in langs)
        {
            if (!Directory.Exists($"{_dir}/{lang}")) Directory.CreateDirectory($"{_dir}/{lang}");
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
                if (!File.Exists($"{_dir}/{lang}/{sheet}.txt"))
                {
                    var t = ((TextAsset)Resources.Load($"Languages/{lang}_{sheet}", typeof(TextAsset))).text;
                    using var outputFile = new StreamWriter($"{_dir}/{lang}/{sheet}.txt");
                    outputFile.Write(t);
                }
        }

        #endregion


        if (_abFa == null)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using Stream s = asm.GetManifestResourceStream("LanguageSupport.Resources.tmprofont-test-smaller");
            if (s == null) return;

            _abFa = AssetBundle.LoadFromStream(s);
            Object.DontDestroyOnLoad(_abFa);
        }

        foreach (var item in _abFa.GetAllAssetNames())
        {
            Log($"ITEM: {item}");
        }

        if (_fa == null && _abFa != null)
        {
            _fa = _abFa.LoadAsset<TMP_FontAsset>("NotoSerif-Regular-4096x2048-0000xD7FF");
            _fa.name = "noto_serif_thai_bold_tmpro";
            Object.DontDestroyOnLoad(_fa);
            _fa.fallbackFontAssets = new List<TMP_FontAsset>();
            _fa.fallbackFontAssets.AddRange(Object.FindObjectsOfType<TMP_FontAsset>(true));
        }

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
            if (File.Exists($"{_dir}/{lang}/{sheetTitle}.txt"))
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
            if (File.Exists($"{_dir}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt"))
            {
                return File.ReadAllText($"{_dir}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt");
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
            if (File.Exists($"{_dir}/{l}/General.txt") || (langs.Contains((SupportedLanguages) l)))
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