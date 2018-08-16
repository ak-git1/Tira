using System.Data.Entity;
using Tira.Logic.Repository.Entities;

namespace Tira.Logic.Repository
{
    /// <summary>
    /// Context for accessing settings in database
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    internal class SettingsContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Settings
        /// </summary>
        public DbSet<Setting> Settings { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsContext"/> class.
        /// </summary>
        public SettingsContext() : base("SQLiteDbConnection")
        {
        }

        #endregion
    }
}
