using System.Windows.Forms;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.ViewModels;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project creation
    /// </summary>
    public class ProjectCreationViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Path to project file
        /// </summary>
        private string _projectPath;

        #endregion

        #region Properties

        /// <summary>
        /// Project name
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(() => Name);
                OnPropertyChanged(() => IsDataValid);
            }        
        }

        /// <summary>
        /// Path to project file
        /// </summary>
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                SetProperty(ref _projectPath, value);
                OnPropertyChanged(() => ProjectPath);
                OnPropertyChanged(() => IsDataValid);
            }
        }

        /// <summary>
        /// Validation for textboxes
        /// </summary>
        public bool IsDataValid => ProjectPath.NotEmpty() && Name.NotEmpty();

        #endregion

        #region Commands

        /// <summary>
        /// Command for project file selection
        /// </summary>
        public INotifyCommand SelectProjectFileCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectCreationViewModel"/> class.
        /// </summary>
        public ProjectCreationViewModel()
        {
            SelectProjectFileCommand = new NotifyCommand(_ => SelectProjectFile());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Selects project file
        /// </summary>
        private void SelectProjectFile()
        {
            SaveFileDialog projectCreationFileDialog = new SaveFileDialog { Filter = Project.ProjectFileExtensionsFilter };
            if (projectCreationFileDialog.ShowDialog() == DialogResult.OK)
                ProjectPath = projectCreationFileDialog.FileName;
        }

        #endregion
    }
}