using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Ak.Framework.Core.Extensions;

namespace Tira.App.Helpers
{
    /// <summary>
    /// Class for changing culture without of restarting application
    /// </summary>
    public class CultureResources
    {
        #region Variables

        /// <summary>
        /// List of registerd resources namings
        /// </summary>
        private static readonly ConcurrentBag<string> ResourceNames = new ConcurrentBag<string>();

        /// <summary>
        /// List of registered resources types (classes of types Resources.Designer)
        /// </summary>
        private static readonly ConcurrentBag<Type> ResourceTypes = new ConcurrentBag<Type>();

        #endregion

        #region Public methods

        /// <summary>
        /// Changing current culture
        /// </summary>
        /// <param name="culture">New culture</param>
        public static void ChangeCulture(CultureInfo culture)
        {
            ResourceTypes.ForEach(t =>
            {
                t.GetProperty("Culture", BindingFlags.Public | BindingFlags.Static)?.SetValue(null, culture);
            });

            Thread.CurrentThread.CurrentCulture = culture;
            Application.Current?.Dispatcher?.Invoke(() => Thread.CurrentThread.CurrentCulture = culture);

            ResourceNames.ForEach(r =>
            {
                (Application.Current.FindResource(r) as ObjectDataProvider)?.Refresh();
            });
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="resourceType">Resource type (class of type Resources.Designer)</param>
        protected CultureResources(string name, Type resourceType)
        {
            ResourceNames.Add(name);
            ResourceTypes.Add(resourceType);
        }

        #endregion

    }
}
