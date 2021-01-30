using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Language;
using UnityEngine;
using UnityEngine.Analytics;
using Logger = Modding.Logger;

namespace LanguageSupport.Consts
{
    public class LanguageStrings
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> jsonDict { get; private set; }
        public Dictionary<int, string> numberToName { get; private set; }
        private readonly string FOLDER = "LanguageSupport";
        private readonly string DIR;

        public LanguageStrings()
        {
            jsonDict = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            numberToName = new Dictionary<int, string>();

            int i = 0;
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
            if (Directory.GetFiles(DIR).Length == 0)
            {
                Log("There are no custom language files in the LanguageSupport directory.");
                return;
            }
            foreach (string file in Directory.GetFiles(DIR))
            {
                var fileInfo = new FileInfo(file);
                string basename = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.'));
                Log($"File: {basename}");

                using (StreamReader sr = fileInfo.OpenText())
                {
                    LanguageCode index;
                    try
                    {
                        index = (LanguageCode)Enum.Parse(typeof(LanguageCode), basename, true);
                    }
                    catch (Exception e)
                    {
                        index = (LanguageCode) 256 + i;
                        i++;
                        numberToName.Add((int)index, basename);
                    }
                    jsonDict.Add(((int)index).ToString(), JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(sr.ReadToEnd()));
                }
            }
        }

        public string Get(string key, string sheet)
        {
            GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
            try
            {
                return jsonDict[lang.ToString()][sheet][key].Replace("<br>", "\n");
            }
            catch
            {
                return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet][key].Replace("<br>", "\n");
            }
        }

        public bool ContainsKey(string key, string sheet)
        {
            try
            {
                GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
                try
                {
                    return jsonDict[lang.ToString()][sheet].ContainsKey(key);
                }
                catch
                {
                    try
                    {
                        return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet].ContainsKey(key);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void Log(string message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message}");
        }
        private void Log(System.Object message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message.ToString()}");
        }
    }
}