using System;
using System.Windows;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Interfaces;
using Tira.App.Logic.Helpers;
using Tira.Logic.Helpers;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
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

        #region Commands

        /// <summary>
        /// Command for project opening
        /// </summary>
        public INotifyCommand OpenProjectCommand { get; }

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

            OpenProjectCommand = new NotifyCommand(OpenProject);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Opens the project
        /// </summary>
        private void OpenProject(object parameters)
        {
            try
            {
                Project project = Project.Load(openFileDialog.FileName);
                OpenProjectWindow(project, (Window)window);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to open project");
                FormsHelper.ShowUnexpectedError();
            }
        }

        #endregion
    }
}
