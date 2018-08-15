using System;
using Ak.Framework.Core.Extensions;
using Tira.Logic.Models;

namespace Tira.App.ViewModels
{
    /// <summary>
    /// ViewModel for recently used project
    /// </summary>
    public class RecentProjectViewModel
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

        #endregion

        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProjectViewModel"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        public RecentProjectViewModel(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProjectViewModel" /> class.
        /// </summary>
        /// <param name="recentProject">Recent project</param>
        public RecentProjectViewModel(RecentProject recentProject)
        {
            Name = recentProject.Name;
            Path = recentProject.Path.TruncateString(200);
        }

        #endregion
    }
}
