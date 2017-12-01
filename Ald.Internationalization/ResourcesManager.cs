using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ald.Internationalization
{
    public abstract class ResourcesManager
    {
        Dictionary<string, Uri> languages = new Dictionary<string, Uri>();

        string currentLanguage;

        public List<string> AllLanguages
        {
            get
            {
                return languages.Keys.ToList();
            }
        }

        public void RegisterLanguage(string language, Uri resourcePath)
        {
            this.languages.Add(language, resourcePath);
        }

        private int FindCurrentLanguagePosition()
        {
            var allUris = this.languages.Values;
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            for (int i = 0; i < dictionaries.Count; i++)
                if (allUris.Contains(dictionaries[i].Source))
                    return i;
            return -1;
        }

        public string CurrentLanguage
        {
            get
            {
                return currentLanguage;
            }
            set
            {
                int position = FindCurrentLanguagePosition();
                var dictionary = new ResourceDictionary();
                dictionary.Source = this.languages[value];
                if (position != -1)
                    Application.Current.Resources.MergedDictionaries[position] = dictionary;
                else
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                this.currentLanguage = value;
            }
        }
    }
}
