using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Ak.Framework.Core.Helpers;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Culture;
using Ak.Framework.Wpf.Dialogs;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Logic.Enums;
using Tira.App.Logic.Events;
using Tira.App.Logic.Helpers;
using Tira.App.Properties;
using Tira.App.Windows;
using Tira.Logic.Enums;
using Tira.Logic.Helpers;
using Tira.Logic.Models;
using Application = System.Windows.Application;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class ProjectViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Gallery images
        /// </summary>
        private BindingList<GalleryImage> _images = new BindingList<GalleryImage>();

        /// <summary>
        /// Current image
        /// </summary>
        private BitmapSource _selectedImage;

        /// <summary>
        /// Current gallery image
        /// </summary>
        private GalleryImage _selectedGalleryImage;

        /// <summary>
        /// Flag for performing operations with image
        /// </summary>
        private bool _imageLoadedToViewer;

        /// <summary>
        /// Data grid columns
        /// </summary>
        private ObservableCollection<DataGridColumn> _dataGridColumns;

        #endregion

        #region Properties

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// Project title
        /// </summary>
        public string ProjectTitle => string.Format(Resources.Ribbon_StatusBar_ProjectTitleFormat, Project.Name);

        /// <summary>
        /// Gallery images
        /// </summary>
        public BindingList<GalleryImage> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged(() => Images);
            }
        }

        /// <summary>
        /// Selected image
        /// </summary>
        public BitmapSource SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(() => SelectedImage);
            }
        }

        /// <summary>
        /// Selected gallery image
        /// </summary>
        public GalleryImage SelectedGalleryImage
        {
            get => _selectedGalleryImage;
            set
            {
                _selectedGalleryImage = value;
                OnPropertyChanged(() => SelectedGalleryImage);
            }
        }

        /// <summary>
        /// Gets the selected images uids.
        /// </summary>
        public List<Guid> SelectedImagesUids { get; private set; }

        /// <summary>
        /// Data grid columns
        /// </summary>
        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get => _dataGridColumns;
            private set
            {
                _dataGridColumns = value;
                OnPropertyChanged(() => DataGridColumns);
            }
        }

        /// <summary>
        /// Flag for performing operations with image
        /// </summary>
        public bool ImageLoadedToViewer
        {
            get => _imageLoadedToViewer;
            set
            {
                _imageLoadedToViewer = value;
                OnPropertyChanged(() => ImageLoadedToViewer);
            }
        }

        /// <summary>
        /// Image viewer zoom manager
        /// </summary>
        public ZoomManager ImageViewerZoomManager { get; set; }

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
        /// Command for project data export
        /// </summary>
        public INotifyCommand ExportProjectDataCommand { get; }

        /// <summary>
        /// Command for settings window showing
        /// </summary>
        public INotifyCommand ShowSettingsWindowCommand { get; }

        /// <summary>
        /// Command for showing help
        /// </summary>
        public INotifyCommand ShowHelpCommand { get; }

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
        /// Command for showing project settings window
        /// </summary>
        public INotifyCommand ShowProjectSettingsWindowCommand { get; }

        #region Gallery commands

        /// <summary>
        /// Command for adding images to gallery
        /// </summary>
        public INotifyCommand AddImagesToGalleryCommand { get; }

        /// <summary>
        /// Command for removing images from gallery
        /// </summary>
        public INotifyCommand RemoveImagesFromGalleryCommand { get; }

        /// <summary>
        /// Command for handling gallery image selection
        /// </summary>
        public INotifyCommand HandleGalleryImageSelectedCommand { get; }

        /// <summary>
        /// Command for handling gallery images ids selection
        /// </summary>
        public INotifyCommand HandleGalleryImagesIdsSelectedCommand { get; }

        #endregion

        #region Image commands

        /// <summary>
        /// Command for image zoom in
        /// </summary>
        public INotifyCommand ImageZoomInCommand { get; }

        /// <summary>
        /// Command for image zoom out
        /// </summary>
        public INotifyCommand ImageZoomOutCommand { get; }

        /// <summary>
        /// Command for image fit size
        /// </summary>
        public INotifyCommand ImageFitSizeCommand { get; }

        /// <summary>
        /// Command for image fit width
        /// </summary>
        public INotifyCommand ImageFitWidthCommand { get; }

        /// <summary>
        /// Command for image fit height
        /// </summary>
        public INotifyCommand ImageFitHeightCommand { get; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectViewModel"/> class.
        /// </summary>
        /// <param name="project">Project</param>
        public ProjectViewModel(Project project)
        {
            Project = project;
            FillGallery();
            FillDataGridColumns();

            CreateProjectCommand = new NotifyCommand(_ => CreateProject());
            OpenProjectCommand = new NotifyCommand(_ => OpenProject());
            SaveProjectCommand = new NotifyCommand(_ => SaveProject());
            ExportProjectDataCommand = new NotifyCommand(o => ExportProjectData((ExportFormat)o));
            ShowSettingsWindowCommand = new NotifyCommand(_ => ShowSettingsWindow());
            ShowHelpCommand = new NotifyCommand(_ => ShowHelp());
            ShowAboutWindowCommand = new NotifyCommand(_ => ShowAboutWindow());
            CloseWindowCommand = new NotifyCommand(o => CloseWindow((Window)o));
            PerformOcrCommand = new NotifyCommand(_ => PerformOcr());
            ShowProjectSettingsWindowCommand = new NotifyCommand(_ => ShowProjectSettingsWindow());

            AddImagesToGalleryCommand = new NotifyCommand(_ => AddImagesToGallery());
            RemoveImagesFromGalleryCommand = new NotifyCommand(_ => RemoveImagesFromGallery());
            HandleGalleryImageSelectedCommand = new NotifyCommand(e => FillGalleryImage((GalleryImageEventArgs)e));
            HandleGalleryImagesIdsSelectedCommand = new NotifyCommand(e => GetGalleryImagesUids((GalleryImagesUidsSelectionEventArgs)e));

            ImageZoomInCommand = new NotifyCommand(_ => ImageZoomIn(), _ => CanPerformOperationsWithImage());
            ImageZoomOutCommand = new NotifyCommand(_ => ImageZoomOut(), _ => CanPerformOperationsWithImage());
            ImageFitSizeCommand = new NotifyCommand(_ => ImageFitSize(), _ => CanPerformOperationsWithImage());
            ImageFitWidthCommand = new NotifyCommand(_ => ImageFitWidth(), _ => CanPerformOperationsWithImage());
            ImageFitHeightCommand = new NotifyCommand(_ => ImageFitHeight(), _ => CanPerformOperationsWithImage());
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
            try
            {
                SettingsViewModel vm = new SettingsViewModel();
                bool? result = ShowDialogAgent.Instance.ShowDialog<SettingsWindow>(vm);
                if (result.HasValue && result.Value)
                {                    
                    CultureInfo.CurrentCulture = new CultureInfo(EnumNamesHelper.GetDescription(vm.SelectedLocaleLanguage));
                    CultureResourcesBase.ChangeCulture(CultureInfo.CurrentCulture);
                    RefreshUiHelper.UpdateRecursive(Application.Current.MainWindow);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to show settings window.");
                FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Shows About window
        /// </summary>
        private void ShowAboutWindow()
        {
            ShowDialogAgent.Instance.ShowDialog<AboutWindow>(new AboutViewModel());
        }

        /// <summary>
        /// Shows help
        /// </summary>
        private void ShowHelp()
        {
            // TODO
            FormsHelper.ShowMessage("Help is opened", "Help");
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window">Window</param>
        private void CloseWindow(Window window)
        {
            window.Close();
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
        /// Exports project data
        /// </summary>
        /// <param name="exportFormat">ExportFormat</param>
        private void ExportProjectData(ExportFormat exportFormat)
        {
            try
            {
                ActionResult result = Project.CheckRecognitionResultExportAllowed();
                if (result.Result == ActionResultType.Error)
                {
                    FormsHelper.ShowError(result.Message);
                }
                else
                {
                    SaveFileDialog exportDataFileDialog = new SaveFileDialog
                    {
                        FileName = $"{Project.Name}.{ExportFormatHelper.GetFileExtension(exportFormat)}",
                        Filter = ExportFormatHelper.GetFileFilterForSaveDialog(exportFormat)
                    };
                    if (exportDataFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Project.Export(exportDataFileDialog.FileName, exportFormat);
                        Process.Start(Path.GetDirectoryName(exportDataFileDialog.FileName));
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.Logger.Error(exception, "Unable to export project recognized data");
            }
        }

        /// <summary>
        /// Shows project settings window
        /// </summary>
        private void ShowProjectSettingsWindow()
        {
            try
            {
                ProjectSettingsViewModel vm = new ProjectSettingsViewModel(Project.DataColumns);
                bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectSettingsWindow>(vm);
                if (result.HasValue && result.Value)
                {
                    Project.DataColumns = new List<DataColumn>(vm.DataColumns);
                    FillDataGridColumns();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to show project settings window.");
                FormsHelper.ShowUnexpectedError();
            }
        }

        #region Gallery operations

        /// <summary>
        /// Adds images to gallery
        /// </summary>
        private void AddImagesToGallery()
        {
            OpenFileDialog imagesImportFileDialog = new OpenFileDialog { Multiselect = true };
            if (imagesImportFileDialog.ShowDialog() == DialogResult.OK)
            {
                Project.Gallery.AddFiles(imagesImportFileDialog.FileNames, Project.DataColumns.Count - 1);
                FillGallery();
            }
        }

        /// <summary>
        /// Removes images from gallery
        /// </summary>
        private void RemoveImagesFromGallery()
        {
            if (SelectedImagesUids != null && SelectedImagesUids.Any())
            {
                Project.Gallery.Images.RemoveAll(r => SelectedImagesUids.Contains(r.Uid));
                if (SelectedImagesUids.Contains(SelectedGalleryImage.Uid))
                    ClearImageViewer();

                FillGallery();
            }
            else
            {
                FormsHelper.ShowMessage(Resources.NoImageToRemoveWarning_Text, Resources.NoImageToRemoveWarning_Caption);
            }
        }

        /// <summary>
        /// Fills the gallery control
        /// </summary>
        private void FillGallery()
        {
            _images = new BindingList<GalleryImage>();
            if (Project.Gallery.Images.Count > 0)
                foreach (GalleryImage galleryImage in Project.Gallery.Images)
                    _images.Add(galleryImage);

            OnPropertyChanged(() => Images);
        }

        #endregion

        #region Image operations

        /// <summary>
        /// Fills the gallery image.
        /// </summary>
        /// <param name="args">The <see cref="GalleryImageEventArgs"/> instance containing the event data.</param>
        private void FillGalleryImage(GalleryImageEventArgs args)
        {
            ImageViewerZoomManager = new ZoomManager();
            SelectedGalleryImage = args.Image;
            SelectedImage = new BitmapImage(new Uri(SelectedGalleryImage.ImageFilePath));
            OnPropertyChanged(() => ImageViewerZoomManager);

            ImageLoadedToViewer = true;
        }

        /// <summary>
        /// Clears image viewer
        /// </summary>
        private void ClearImageViewer()
        {
            ImageViewerZoomManager = new ZoomManager();
            SelectedGalleryImage = null;
            SelectedImage = null;
            OnPropertyChanged(() => ImageViewerZoomManager);

            ImageLoadedToViewer = false;
        }

        /// <summary>
        /// Gets gallery images uids 
        /// </summary>
        /// <param name="args">The <see cref="GalleryImagesUidsSelectionEventArgs"/> instance containing the event data.</param>
        private void GetGalleryImagesUids(GalleryImagesUidsSelectionEventArgs args)
        {
            SelectedImagesUids = args.Uids;
        }

        /// <summary>
        /// Checks if operations with image in viewer can be performed
        /// </summary>
        /// <returns></returns>
        private bool CanPerformOperationsWithImage()
        {
            return ImageLoadedToViewer && SelectedGalleryImage != null && SelectedImage != null;
        }

        /// <summary>
        /// Zoom in image in image viewer
        /// </summary>
        private void ImageZoomIn()
        {
            ImageViewerZoomManager.ZoomIn();
            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        /// <summary>
        /// Zoom out image in image viewer
        /// </summary>
        private void ImageZoomOut()
        {
            ImageViewerZoomManager.ZoomOut();
            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        /// <summary>
        /// Fit image by size in image viewer
        /// </summary>
        private void ImageFitSize()
        {
            ImageViewerZoomManager.FitMode = FitOption.FitSize;
            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        /// <summary>
        /// Fit image by height in image viewer
        /// </summary>
        private void ImageFitHeight()
        {
            ImageViewerZoomManager.FitMode = FitOption.FitHeight;
            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        /// <summary>
        /// Fit image by width in image viewer
        /// </summary>
        private void ImageFitWidth()
        {
            ImageViewerZoomManager.FitMode = FitOption.FitWidth;
            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        #endregion

        #region DataGrid operations

        /// <summary>
        /// Fills the data grid columns.
        /// </summary>
        private void FillDataGridColumns()
        {
            DataGridColumns = new ObservableCollection<DataGridColumn>();
            foreach (DataColumn dataColumn in Project.DataColumns)
                DataGridColumns.Add(new DataGridTextColumn
                {
                    Header = dataColumn.Name,
                    Width = 150
                });
            OnPropertyChanged(() => DataGridColumns);
        }        

        #endregion

        #endregion
    }
}