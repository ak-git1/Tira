using System.Collections.Generic;
using System.Windows;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Dialogs;
using Tira.App.Windows;
using Tira.Logic.Models;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for projects templates
    /// </summary>
    /// <seealso cref="Tira.App.Logic.ViewModels.TiraViewModelBase" />
    public class ProjectTemplatesViewModel : TiraViewModelBase
    {
        #region Properties

        /// <summary>
        /// Project templates list
        /// </summary>
        public List<ProjectTemplate> ProjectTemplates { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command for editing project template
        /// </summary>
        public INotifyCommand EditProjectTemplateCommand { get; }

        /// <summary>
        /// Command for deleting data column
        /// </summary>
        public INotifyCommand DeleteProjectTemplateCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplatesViewModel"/> class.
        /// </summary>
        public ProjectTemplatesViewModel()
        {
            ProjectTemplates = ProjectTemplate.GetList();

            EditProjectTemplateCommand = new NotifyCommand(o => EditProjectTemplate((ProjectTemplate)o));
            DeleteProjectTemplateCommand = new NotifyCommand(o => DeleteProjectTemplate((ProjectTemplate)o));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Edit project template
        /// </summary>
        /// <param name="projectTemplate">Project template</param>
        private void EditProjectTemplate(ProjectTemplate projectTemplate)
        {
            ProjectTemplateViewModel vm = new ProjectTemplateViewModel(projectTemplate);
            bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectTemplateWindow>(vm);
            if (result.HasValue && result.Value)
            {
                projectTemplate.Name = vm.Name;
                projectTemplate.Update();

                ProjectTemplates = ProjectTemplate.GetList();
                OnPropertyChanged(() => ProjectTemplates);
            }
        }

        /// <summary>
        /// Deletes data column
        /// </summary>
        /// <param name="projectTemplate">Project template</param>
        private void DeleteProjectTemplate(ProjectTemplate projectTemplate)
        {
            if (projectTemplate != null)
                if (MessageBox.Show(Properties.Resources.ProjectTemplateDelete_Text, Properties.Resources.ProjectTemplateDelete_Caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ProjectTemplate.Delete(projectTemplate.Id);
                    ProjectTemplates = ProjectTemplate.GetList();
                    OnPropertyChanged(() => ProjectTemplates);
                }
        }

        #endregion
    }
}
