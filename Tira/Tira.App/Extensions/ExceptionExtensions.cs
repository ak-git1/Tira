using System;
using Tira.App.Properties;
using Tira.App.ViewModels;
using Tira.App.Windows;

namespace Tira.App.Extensions
{
    /// <summary>
    /// Extensions for exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Shows exception and exit application
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void ShowAndExitApplication(this Exception exception)
        {
            ExceptionWindow showExceptionWindow = new ExceptionWindow
            {
                DataContext = new ExceptionViewModel(Resources.UnhandledException, Resources.UnhandledExceptionOccurredApplicationWillBeClosed, exception)
            };
            showExceptionWindow.ShowDialog();
            Environment.Exit(-1);
        }
    }
}