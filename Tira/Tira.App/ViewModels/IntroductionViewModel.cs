using System;
using System.Collections.Generic;
using System.Reflection;
using Tira.App.Properties;
using Tira.Logic.Models;

namespace Tira.App.ViewModels
{
    /// <summary>
    /// ViewModel for introduction window
    /// </summary>
    public class IntroductionViewModel
    {
        #region Properties

        /// <summary>
        /// Application version
        /// </summary>
        public string Version => string.Format(Resources.IntroductionWindow_Label_Version, Assembly.GetEntryAssembly().GetName().Version);

        /// <summary>
        /// Recent projects
        /// </summary>
        public List<RecentProjectViewModel> RecentProjects { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IntroductionViewModel"/> class.
        /// </summary>
        public IntroductionViewModel()
        {
            RecentProjects = new List<RecentProjectViewModel>
            {
                new RecentProjectViewModel("Project 1", "C:\\project1.proj"),
                new RecentProjectViewModel("Project 2", "C:\\project2.proj"),
                new RecentProjectViewModel("Project 3", "C:\\project3.proj"),
            };
        }

        #endregion
    }
}
