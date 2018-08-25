using System.Windows;
using Tira.App.Properties;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Tira.App.Logic.Helpers
{
    /// <summary>
    /// Class for execution operation with forms
    /// </summary>
    public static class FormsHelper
    {
        #region Public methods

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public static void ShowMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK);
        }

        /// <summary>
        /// Shows the warning.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public static void ShowWarning(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Shows unexpected error.
        /// </summary>
        public static void ShowUnexpectedError()
        {
            MessageBox.Show(Resources.UnexpectedError_Text, Resources.UnexpectedError_Caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows error.
        /// </summary>
        public static void ShowError(string text)
        {
            MessageBox.Show(text, Resources.Error_Caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
