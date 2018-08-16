using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ak.Framework.Core.Extensions;
using Tira.Logic.Helpers;
using Tira.Logic.Repository;
using Tira.Logic.Repository.Entities;

namespace Tira.Logic.Settings
{
    /// <summary>
    /// Class for working with settings
    /// </summary>
    internal static class SettingsHelper
    {
        #region Public methods

        /// <summary>
        /// Gets the value of setting by name
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            try
            {
                using (SettingsContext db = new SettingsContext())
                {
                    Setting s = db.Settings.FirstOrDefault(x => x.Name.Equals(name));
                    return s == null ? string.Empty : s.Value;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to get settings value from database.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the value of setting
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void SetValue(string name, string value)
        {
            try
            {
                using (SettingsContext db = new SettingsContext())
                {
                    Setting s = db.Settings.FirstOrDefault(x => x.Name.Equals(name));
                    if (s != null)
                    {
                        s.Value = value;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to save settings to database.");
            }
        }

        #endregion
    }
}