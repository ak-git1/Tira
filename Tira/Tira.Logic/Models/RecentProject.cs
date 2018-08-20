using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Framework.Core.Extensions;
using Tira.Logic.Helpers;
using Tira.Logic.Repository;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Recently used project
    /// </summary>
    public class RecentProject
    {
        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Shorten path
        /// </summary>
        public string ShortPath => Path.TruncateString(100);

        /// <summary>
        /// Last access date
        /// </summary>
        public DateTime LastAccessDate { get; }

        #endregion

        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        /// <param name="lastAccessDate">Last access date.</param>
        public RecentProject(string name, string path, DateTime lastAccessDate)
        {
            Name = name;
            Path = path;
            LastAccessDate = lastAccessDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        public RecentProject(string name, string path)
        {
            Name = name;
            Path = path;
            LastAccessDate = DateTime.Now;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets list of recent projects
        /// </summary>
        /// <param name="number">The number of items retrived</param>
        /// <returns></returns>
        public static List<RecentProject> GetList(int number)
        {
            List<RecentProject> result = new List<RecentProject>();

            try
            {                
                using (RecentProjectsContext db = new RecentProjectsContext())
                {
                    foreach (Repository.Entities.RecentProject p in db.RecentProjects.OrderByDescending(x => x.LastAccessDate).Take(number))
                        result.Add(new RecentProject(p.Name, p.Path, p.LastAccessDate.ToDateTime()));
                }                
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to get recent projects list from database.");
            }

            return result;
        }

        /// <summary>
        /// Saving recent project to database
        /// </summary>
        public void AddOrUpdate()
        {
            using (RecentProjectsContext db = new RecentProjectsContext())
            {
                Repository.Entities.RecentProject p = db.RecentProjects.FirstOrDefault(x => x.Path.Equals(Path));
                if (p == null)
                    db.RecentProjects.Add(new Repository.Entities.RecentProject
                    {
                        Name = Name,
                        Path = Path,
                        LastAccessDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                else
                    p.LastAccessDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes recent project by path
        /// </summary>
        /// <param name="path">Path.</param>
        public static void Delete(string path)
        {
            using (RecentProjectsContext db = new RecentProjectsContext())
            {
                Repository.Entities.RecentProject p = db.RecentProjects.FirstOrDefault(x => x.Path.Equals(path));
                if (p != null)
                {
                    db.RecentProjects.Remove(p);
                    db.SaveChanges();
                }
            }
        }

        #endregion
    }
}