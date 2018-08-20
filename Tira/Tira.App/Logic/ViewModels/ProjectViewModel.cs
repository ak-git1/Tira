using Ak.Framework.Wpf.ViewModels;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class ProjectViewModel : ViewModelBase
    {
        #region Properties

        private Project Project { get; }

        #endregion

        #region Constructor

        public ProjectViewModel(Project project)
        {
            Project = project;
        }

        #endregion
    }
}
