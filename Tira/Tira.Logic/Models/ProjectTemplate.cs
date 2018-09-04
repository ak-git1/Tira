using System;
using System.Collections.Generic;
using System.Linq;
using Tira.Logic.Helpers;
using Tira.Logic.Repository;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Project template
    /// </summary>
    public class ProjectTemplate
    {
        #region Properties

        /// <summary>
        /// Identificator
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; set; }

        #endregion

        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject" /> class.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Name.</param>
        /// <param name="data">Path.</param>
        public ProjectTemplate(int id, string name, string data)
        {
            Id = id;
            Name = name;
            Data = data;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets list of project templates
        /// </summary>
        /// <returns></returns>
        public static List<ProjectTemplate> GetList()
        {
            List<ProjectTemplate> result = new List<ProjectTemplate>();

            try
            {
                using (ProjectTemplatesContext db = new ProjectTemplatesContext())
                {
                    foreach (Repository.Entities.ProjectTemplate p in db.ProjectTemplates.OrderBy(x => x.Name))
                        result.Add(new ProjectTemplate(p.Id, p.Name, p.Data));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to get projects templates list from database.");
            }

            return result;
        }

        /// <summary>
        /// Add project template to database
        /// </summary>
        public void Add()
        {
            using (ProjectTemplatesContext db = new ProjectTemplatesContext())
            {
                db.ProjectTemplates.Add(new Repository.Entities.ProjectTemplate
                {
                    Name = Name,
                    Data = Data
                });
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Check project templates existance by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            try
            {
                using (ProjectTemplatesContext db = new ProjectTemplatesContext())
                {
                    Repository.Entities.ProjectTemplate p = db.ProjectTemplates.FirstOrDefault(x => x.Name.Equals(name));
                    return p != null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to get projects templates list from database.");
                return false;
            }
        }

        /// <summary>
        /// Deletes project template by id
        /// </summary>
        /// <param name="id">Id</param>
        public static void Delete(int id)
        {
            using (ProjectTemplatesContext db = new ProjectTemplatesContext())
            {
                Repository.Entities.ProjectTemplate t = db.ProjectTemplates.FirstOrDefault(x => x.Id == id);
                if (t != null)
                {
                    db.ProjectTemplates.Remove(t);
                    db.SaveChanges();
                }
            }
        }

        #endregion
    }
}