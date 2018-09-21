using System;
using System.Diagnostics;
using System.IO;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Logic.Helpers;
using Tira.App.Properties;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// Abstract ViewModel for handling common functionality
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public abstract class TiraViewModelBase : ViewModelBase
    {
        #region Commands

        /// <summary>
        /// Command for showing help
        /// </summary>
        public INotifyCommand ShowHelpCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TiraViewModelBase"/> class.
        /// </summary>
        public TiraViewModelBase()
        {
            ShowHelpCommand = new NotifyCommand(_ => ShowHelp());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Shows help
        /// </summary>
        private void ShowHelp()
        {
            try
            {
                Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Help\Tira-Help.pdf"));
            }
            catch
            {
                FormsHelper.ShowError(Resources.HelpFileNotFound);
            }
        }

        #endregion
    }
}
