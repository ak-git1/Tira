using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Tira.App.Logic.Extensions;
using Tira.App.Logic.Helpers;
using Tira.App.Logic.ViewModels;
using Tira.App.Windows;
using Tira.Logic.Helpers;
using Tira.Logic.Settings;

namespace Tira.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Variables

        #region Mechanism of interception unhandled exceptions in AppDomain

        /// <summary>
        /// Blocker object for setting mechanism of interception unhandled exceptions in AppDomain
        /// </summary>
        private static readonly object DomainExceptionLock = new object();

        /// <summary>
        /// Flag showong that domain exception interception is set
        /// </summary>
        private static bool _isDomainExceptionIntercepted;

        #endregion

        #endregion

        #region Private methods
        
        /// <summary>
        /// Setting handler of the last exception
        /// </summary>
        private static void InterceptDomainExceptions()
        {
            lock (DomainExceptionLock)
            {
                if (_isDomainExceptionIntercepted)
                    return;

                AppDomain.CurrentDomain.UnhandledException += (obj, args) =>
                {
                    if (args.ExceptionObject is Exception exception)
                        LogHelper.Logger.Fatal(exception, "Unhandled exception in current application domain! See error log for details.");
                    else
                        LogHelper.Logger.Fatal("Unhandled exception of unknown type in current application domain! ToString == " + args.ExceptionObject);

                    LogHelper.Flush();
                };
                _isDomainExceptionIntercepted = true;
            }
        }

        protected static void OnUnhandledException(Exception ex)
        {
            LogHelper.Logger.Error(ex, "Dispatcher unhandled exception thrown!");
            ex.ShowAndExitApplication();
        }

        #endregion

        #region Event handlers

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnUnhandledException(e.ExceptionObject as Exception);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            OnUnhandledException(e.Exception);
        }

        protected virtual void OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(LanguageSettings.CurrentLanguageCode);
                CultureResources.ChangeCulture(new CultureInfo(LanguageSettings.CurrentLanguageCode));
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to read locale settings");
            }

            InterceptDomainExceptions();

            try
            {
                new IntroductionWindow(new IntroductionViewModel()).Show();
            }
            catch (Exception exception)
            {
                OnUnhandledException(exception);
            }
        }

        #endregion
    }
}
