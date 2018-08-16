using System.Collections.Generic;
using System.Reflection;
using Tira.App.Properties;
using Tira.Logic.Models;
using Tira.Logic.Settings;

namespace Tira.App.Logic.ViewModels
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
            RecentProjects = new List<RecentProjectViewModel>();
            foreach (RecentProject p in RecentProject.GetList(CommonSettings.MaxNumberOfRecentProjects))
                RecentProjects.Add(new RecentProjectViewModel(p.Name, p.Path));
        }

        #endregion
    }
}
