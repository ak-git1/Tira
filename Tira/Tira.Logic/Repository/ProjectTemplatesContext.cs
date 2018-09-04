using System.Data.Entity;
using Tira.Logic.Repository.Entities;

namespace Tira.Logic.Repository
{
    /// <summary>
    /// Context for accessing project templates in database
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    internal class ProjectTemplatesContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Project templates
        /// </summary>
        public DbSet<ProjectTemplate> ProjectTemplates { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplatesContext"/> class.
        /// </summary>
        public ProjectTemplatesContext() : base("SQLiteDbConnection")
        {
        }

        #endregion
    }
}
