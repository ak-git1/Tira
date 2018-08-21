using System;
using System.Windows;
using System.Windows.Forms;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Dialogs;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Logic.Helpers;
using Tira.App.Properties;
using Tira.App.Windows;
using Tira.Logic.Enums;
using Tira.Logic.Helpers;
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

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// Project title
        /// </summary>
        public string ProjectTitle => string.Format(Properties.Resources.Ribbon_StatusBar_ProjectTitleFormat, Project.Name);

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

        /// <summary>
        /// Command for project saving
        /// </summary>
        public INotifyCommand SaveProjectCommand { get; }

        /// <summary>
        /// Command for settings window showing
        /// </summary>
        public INotifyCommand ShowSettingsWindowCommand { get; }

        /// <summary>
        /// Command for about window showing
        /// </summary>
        public INotifyCommand ShowAboutWindowCommand { get; }

        /// <summary>
        /// Command for window closing
        /// </summary>
        public INotifyCommand CloseWindowCommand { get; }

        /// <summary>
        /// Command for perform OCR
        /// </summary>
        public INotifyCommand PerformOcrCommand { get; }

        /// <summary>
        /// Command for adding images to gallery
        /// </summary>
        public INotifyCommand AddImagesToGalleryCommand { get; }

        /// <summary>
        /// Command for removing images from gallery
        /// </summary>
        public INotifyCommand RemoveImagesFromGalleryCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectViewModel"/> class.
        /// </summary>
        /// <param name="project">Project</param>
        public ProjectViewModel(Project project)
        {
            Project = project;

            CreateProjectCommand = new NotifyCommand(_ => CreateProject());
            OpenProjectCommand = new NotifyCommand(_ => OpenProject());
            SaveProjectCommand = new NotifyCommand(_ => SaveProject());
            ShowSettingsWindowCommand = new NotifyCommand(_ => ShowSettingsWindow());
            ShowAboutWindowCommand = new NotifyCommand(_ => ShowAboutWindow());
            CloseWindowCommand = new NotifyCommand(CloseWindow);
            PerformOcrCommand = new NotifyCommand(_ => PerformOcr());
            AddImagesToGalleryCommand = new NotifyCommand(_ => AddImagesToGallery());
            RemoveImagesFromGalleryCommand = new NotifyCommand(_ => RemoveImagesFromGallery());
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the project
        /// </summary>
        private void CreateProject()
        {
            try
            {
                ProjectCreationViewModel vm = new ProjectCreationViewModel();
                bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectCreationWindow>(vm);
                if (result.HasValue && result.Value)
                    Project = Project.Create(vm.ProjectPath, vm.Name);
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
        private void OpenProject()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    Project = Project.Load(openFileDialog.FileName);     
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to create project");
                FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Saves the project
        /// </summary>
        private void SaveProject()
        {
            Project.Save();
        }

        /// <summary>
        /// Shows settings window
        /// </summary>
        private void ShowSettingsWindow()
        {
            // TODO            
        }

        /// <summary>
        /// Shows About window
        /// </summary>
        private void ShowAboutWindow()
        {
            ShowDialogAgent.Instance.ShowDialog<AboutWindow>(new AboutViewModel());
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window">Window</param>
        private void CloseWindow(object window)
        {
            ((Window)window).Close();
        }

        /// <summary>
        /// Performs OCR
        /// </summary>
        private void PerformOcr()
        {
            ActionResult result = Project.ValidateOcr();
            if (result.Result == ActionResultType.Error)
            {
                FormsHelper.ShowError(result.Message);
            }
            else
            {
                result = Project.PerformOcr();
                if (result.Result == ActionResultType.Ok)
                {
                    FormsHelper.ShowMessage(Resources.ProjectWindow_OcrCompletedMessage_Text, Resources.ProjectCreationWindow_ProjectValidationMessage_Caption);
                    // TODO - fill recognized data - see example
                }
                else
                    FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Adds images to gallery
        /// </summary>
        private void AddImagesToGallery()
        {

        }

        /// <summary>
        /// Removes images from gallery
        /// </summary>
        private void RemoveImagesFromGallery()
        {

        }

        #endregion
    }
}