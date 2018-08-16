using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Helpers;
using Tira.Logic.Enums;
using Tira.Logic.Properties;

namespace Tira.Logic.Settings
{
    /// <summary>
    /// Language settings
    /// </summary>
    public static class LanguageSettings
    {
        #region Variables

        /// <summary>
        /// Available interface languages
        /// </summary>
        private static List<Languages> _availableLanguages;

        #endregion

        #region Properties

        /// <summary>
        /// Current language
        /// </summary>
        public static Languages CurrentLanguage
        {
            get => (Languages)Properties.Settings.Default.Language;
            set
            {
                Properties.Settings.Default.Language = (int)value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Code of current language
        /// </summary>
        public static string CurrentLanguageCode => EnumNamesHelper.GetDescription(CurrentLanguage);

        /// <summary>
        /// Available languages 
        /// </summary>
        public static List<Languages> AvailableLanguages
        {
            get
            {
                if (_availableLanguages == null)
                {
                    _availableLanguages = new List<Languages>();
                    string[] langs = ConfigurationManager.AppSettings["AvailableLanguages"].ToStr().RemoveSpaces().Split(',');
                    foreach (Languages? lang in langs.Select(l => l.ToEnum<Languages>(null)).Where(l => l.HasValue))
                        _availableLanguages.Add(lang.Value);
                    _availableLanguages = _availableLanguages.Distinct().ToList();
                }
                return _availableLanguages;
            }
        }

        /// <summary>
        /// List of available languages
        /// </summary>
        public static List<KeyValuePair<Languages, string>> AvailableLanguagesList => AvailableLanguages.Select(l => new KeyValuePair<Languages, string>(l, GetLanguageName(l))).ToList();

        /// <summary>
        /// Gets current culture
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetCurrentCulture()
        {
            if (CurrentLanguage == Languages.None)
                return Thread.CurrentThread.CurrentCulture;

            return new CultureInfo(EnumNamesHelper.GetDescription(CurrentLanguage));
        }

        /// <summary>
        /// Gets language name
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns></returns>
        public static string GetLanguageName(Languages language)
        {
            switch (language)
            {
                case Languages.En:
                    return Resources.ResourceManager.GetString("Language_En", GetCurrentCulture());

                case Languages.Ru:
                    return Resources.ResourceManager.GetString("Language_Ru", GetCurrentCulture());
 
                default:
                    return Resources.ResourceManager.GetString("Language_Unknown", GetCurrentCulture());
            }
        }

        #endregion
    }
}
