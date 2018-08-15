using System;
using Ak.Framework.Core.Extensions;

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

        #endregion
    }
}