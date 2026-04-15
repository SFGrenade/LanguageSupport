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
using SFCore.Generics;

namespace LanguageSupport;

[UsedImplicitly]
public class LsGlobalSettings
{
    public LanguageCode lastSelectedLanguage = LanguageCode.N;
}

[UsedImplicitly]
public class LanguageSupport : GlobalSettingsMod<LsGlobalSettings>
{
    internal static LanguageSupport Instance;
    private readonly string _dir;

    private readonly string _folder = "LanguageSupport";

    public LanguageSupport() : base("Language Support")
    {
        Log("!LanguageSupport");
        Instance = this;

        _dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException("I have no idea how you did this, but good luck figuring it out."), _folder);

        if (!Directory.Exists(_dir))
        {
            Directory.CreateDirectory(_dir);
        }

        InitCallbacks();
        InitLanguage();
    }

    public override string GetVersion() => SFCore.Utils.Util.GetVersion(Assembly.GetExecutingAssembly());

    public override void Initialize()
    {
        Log("!Initialize");
        Instance = this;

        // todo: fixme: reload language from global

        foreach (var t in Object.FindObjectsOfType<UnityEngine.UI.MenuLanguageSetting>()) t.RefreshControls();

        Log("Initialized");
    }

    private void InitCallbacks()
    {
        Log("!InitCallbacks");
        // Hooks
        On.Language.Language.HasLanguageFile += OnLanguageHasLanguageFile;
        On.Language.Language.GetLanguageFileContents += OnLanguageGetLanguageFileContents;
        On.UnityEngine.UI.MenuLanguageSetting.RefreshAvailableLanguages += OnMenuLanguageSettingRefreshAvailableLanguages;
        On.UnityEngine.UI.MenuLanguageSetting.PushUpdateOptionList += OnMenuLanguageSettingPushUpdateOptionList;

        On.ChangeFontByLanguage.SetFont += OnChangeFontByLanguageSetFont;
        Log("~InitCallbacks");
    }

    private static AssetBundle _abFa = null;
    private static TMP_FontAsset _fa = null;
    private static Sprite _atlas = null;

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
        Log("!InitLanguage");
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
            using Stream s = asm.GetManifestResourceStream("LanguageSupport.Resources.58k_characters_fixed_compressed");
            if (s == null) return;

            _abFa = AssetBundle.LoadFromStream(s);
            Object.DontDestroyOnLoad(_abFa);
        }

        if (_fa == null && _abFa != null)
        {
            _fa = _abFa.LoadAsset<TMP_FontAsset>("NotoSerif-Regular");
            Object.DontDestroyOnLoad(_fa);
        }

        Language.Language.LoadAvailableLanguages();
        Language.Language.LoadLanguage();
        Log("~InitLanguage");
    }

    private bool OnLanguageHasLanguageFile(On.Language.Language.orig_HasLanguageFile orig, string lang, string sheetTitle)
    {
        Log("!OnLanguageHasLanguageFile");
        var ret = orig(lang, sheetTitle);
        if (!ret)
        {
            if (File.Exists($"{_dir}/{lang}/{sheetTitle}.txt"))
            {
                ret = true;
            }
        }

        Log("~OnLanguageHasLanguageFile");
        return ret;
    }

    private string OnLanguageGetLanguageFileContents(On.Language.Language.orig_GetLanguageFileContents orig, string sheetTitle)
    {
        Log("!OnLanguageGetLanguageFileContents");
        string ret = orig(sheetTitle);
        if (ret == string.Empty)
        {
            if (File.Exists($"{_dir}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt"))
            {
                Log("~OnLanguageGetLanguageFileContents");
                return File.ReadAllText($"{_dir}/{Language.Language.CurrentLanguage()}/{sheetTitle}.txt");
            }
        }

        Log("~OnLanguageGetLanguageFileContents");
        return ret;
    }

    private void OnMenuLanguageSettingRefreshAvailableLanguages(On.UnityEngine.UI.MenuLanguageSetting.orig_RefreshAvailableLanguages orig, UnityEngine.UI.MenuLanguageSetting self)
    {
        Log("!OnMenuLanguageSettingRefreshAvailableLanguages");
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

        SupportedLanguages[] langs = ReflectionHelper.GetField<UnityEngine.UI.MenuLanguageSetting, SupportedLanguages[]>(self, "langs");
        List<SupportedLanguages> finalLangs = new List<SupportedLanguages>();

        foreach (var l in customLangs)
        {
            if (File.Exists($"{_dir}/{l}/General.txt") || (langs.Contains((SupportedLanguages) l)))
            {
                finalLangs.Add((SupportedLanguages) l);
            }
        }

        ReflectionHelper.SetField<UnityEngine.UI.MenuLanguageSetting, SupportedLanguages[]>(self, "langs", finalLangs.ToArray());
        Log("~OnMenuLanguageSettingRefreshAvailableLanguages");
    }

    private void OnMenuLanguageSettingPushUpdateOptionList(On.UnityEngine.UI.MenuLanguageSetting.orig_PushUpdateOptionList orig, UnityEngine.UI.MenuLanguageSetting self)
    {
        Log("!OnMenuLanguageSettingPushUpdateOptionList");
        orig(self);
        SupportedLanguages[] langs = ReflectionHelper.GetField<UnityEngine.UI.MenuLanguageSetting, SupportedLanguages[]>(self, "langs");
        string[] array = new string[langs.Length];
        for (int i = 0; i < langs.Length; i++)
        {
            array[i] = ((LanguageCode) langs[i]).ToString();
        }

        self.SetOptionList(array);
        Log("~OnMenuLanguageSettingPushUpdateOptionList");
    }

    private void OnChangeFontByLanguageSetFont(On.ChangeFontByLanguage.orig_SetFont orig, ChangeFontByLanguage self)
    {
        Log("!OnChangeFontByLanguageSetFont");
        orig(self);

        if (_fa == null) return;
        var tmp = ReflectionHelper.GetField<ChangeFontByLanguage, TextMeshPro>(self, "tmpro");
        var tmpMat = _fa.material;
        tmpMat.shader = tmp.font.material.shader;

        _fa.material = tmpMat;
        tmp.font = _fa;

        ReflectionHelper.SetField<ChangeFontByLanguage, TextMeshPro>(self, "tmpro", tmp);
        Log("~OnChangeFontByLanguageSetFont");
    }
}
