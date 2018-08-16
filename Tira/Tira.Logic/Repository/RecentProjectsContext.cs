using System.Data.Entity;
using Tira.Logic.Repository.Entities;

namespace Tira.Logic.Repository
{
    /// <summary>
    /// Context for accessing recently used projects in database
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    internal class RecentProjectsContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Recent projects
        /// </summary>
        public DbSet<RecentProject> RecentProjects { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProjectsContext"/> class.
        /// </summary>
        public RecentProjectsContext() : base("SQLiteDbConnection")
        {
        }

        #endregion
    }
}
