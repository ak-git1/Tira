﻿using System.Collections.Generic;
using System.Windows.Forms;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project creation
    /// </summary>
    /// <seealso cref="Tira.App.Logic.ViewModels.TiraViewModelBase" />
    public class ProjectCreationViewModel : TiraViewModelBase
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

        /// <summary>
        /// Selected project template
        /// </summary>
        private ProjectTemplate _selectedProjectTemplate;

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
        /// Projects templates list
        /// </summary>
        public List<ProjectTemplate> ProjectTemplatesList { get; }

        /// <summary>
        /// Selected project template
        /// </summary>
        public ProjectTemplate SelectedProjectTemplate
        {
            get => _selectedProjectTemplate;
            set => _selectedProjectTemplate = value.Id == 0 ? null : value;
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
            ProjectTemplatesList = ProjectTemplate.GetList();
            ProjectTemplatesList.Insert(0, new ProjectTemplate(0, Properties.Resources.ProjectTemplate_NotSet, string.Empty));

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