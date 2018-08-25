using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.ViewModels;
using Tira.Logic.Enums;
using Tira.Logic.Settings;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for settings window
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class SettingsViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Выбранный язык
        /// </summary>
        private Languages _selectedLocaleLanguage;

        #endregion

        #region Properties        

        /// <summary>
        /// Selected language
        /// </summary>
        public Languages SelectedLocaleLanguage
        {
            get => _selectedLocaleLanguage;
            set
            {
                SetProperty(ref _selectedLocaleLanguage, value);
                OnPropertyChanged(() => SelectedLocaleLanguageCode);
            }
        }

        /// <summary>
        /// Selected language code
        /// </summary>
        public string SelectedLocaleLanguageCode
        {
            get => SelectedLocaleLanguage.ToString();
            set
            {
                Languages lang = AvailableLanguagesList.Select(p => p.Key).FirstOrDefault(k => k.ToString().Equals(value, StringComparison.OrdinalIgnoreCase));

                if (lang == default(Languages))
                    return;

                SelectedLocaleLanguage = lang;
            }
        }

        /// <summary>
        /// Available languages list
        /// </summary>
        public List<KeyValuePair<Languages, string>> AvailableLanguagesList => LanguageSettings.AvailableLanguagesList;

        #endregion

        #region Commands

        /// <summary>
        /// Command for saving settings
        /// </summary>
        public INotifyCommand SaveSettingsCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            LoadSettings();

            SaveSettingsCommand = new NotifyCommand(_ => SaveSettings());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads settings
        /// </summary>
        private void LoadSettings()
        {
            SelectedLocaleLanguage = LanguageSettings.CurrentLanguage;
        }

        /// <summary>
        /// Saves  settings
        /// </summary>
        private void SaveSettings()
        {
            LanguageSettings.CurrentLanguage = SelectedLocaleLanguage;
        }

        #endregion
    }
}
