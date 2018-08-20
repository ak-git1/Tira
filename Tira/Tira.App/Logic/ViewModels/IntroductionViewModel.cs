using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Ak.Framework.Core.Helpers;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Dialogs;
using Ak.Framework.Wpf.Messaging;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Logic.Enums;
using Tira.App.Logic.Helpers;
using Tira.App.Properties;
using Tira.App.Windows;
using Tira.Logic.Helpers;
using Tira.Logic.Models;
using Tira.Logic.Settings;
using Message = Ak.Framework.Wpf.Messaging.Message;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for introduction window
    /// </summary>
    public class IntroductionViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Recent projects
        /// </summary>
        private List<RecentProjectViewModel> _recentProjects;

        #endregion

        #region Properties

        /// <summary>
        /// Application version
        /// </summary>
        public string Version => string.Format(Resources.IntroductionWindow_Label_Version, AssemblyInfoHelper.GetMainAssemblyVersion());

        /// <summary>
        /// Recent projects
        /// </summary>
        public List<RecentProjectViewModel> RecentProjects
        {
            get => _recentProjects;
            private set
            {
                _recentProjects = value;
                OnPropertyChanged(() => RecentProjects);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command for project creation
        /// </summary>
        public INotifyCommand CreateProjectCommand { get; }

        /// <summary>
        /// Command for project opening
        /// </summary>
        public INotifyCommand OpenProjectCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IntroductionViewModel"/> class.
        /// </summary>
        public IntroductionViewModel()
        {
            ReloadRecentProjectList();

            CreateProjectCommand = new NotifyCommand(CreateProject);
            OpenProjectCommand = new NotifyCommand(OpenProject);

            Messenger.Instance.Register(MessageType.RecentProjectDeleted, ReloadRecentProjectList);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the project
        /// </summary>
        /// <param name="window">The window.</param>
        private void CreateProject(object window)
        {
            try
            {
                ProjectCreationViewModel vm = new ProjectCreationViewModel();
                bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectCreationWindow>(vm);
                if (result.HasValue && result.Value)
                {
                    Project project = Project.Create(vm.ProjectPath, vm.Name);
                    OpenProjectWindow(project, (Window)window);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to create project");
                FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Opens the project
        /// </summary>
        /// <param name="window">Window</param>
        private void OpenProject(object window)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Project project = Project.Load(openFileDialog.FileName);
                    OpenProjectWindow(project, (Window)window);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to create project");
                FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Opens the project window and closes current
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="currentWindow">Current window</param>
        private void OpenProjectWindow(Project project, Window currentWindow)
        {
            currentWindow.Hide();
            new ProjectWindow(new ProjectViewModel(project)).Show();
            currentWindow.Close();
        }

        /// <summary>
        /// Reloads the recent project list.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="objects">Objects.</param>
        private void ReloadRecentProjectList(Message message = null, object[] objects = null)
        {
            RecentProjects = new List<RecentProjectViewModel>();
            foreach (RecentProject p in RecentProject.GetList(CommonSettings.MaxNumberOfRecentProjects))
                RecentProjects.Add(new RecentProjectViewModel(p));
        }

        #endregion
    }
}
