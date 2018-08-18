using Ak.Framework.Wpf.ViewModels;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project creation
    /// </summary>
    public class ProjectCreationViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to project file
        /// </summary>
        public string ProjectPath { get; set; }

        #endregion
    }
}
