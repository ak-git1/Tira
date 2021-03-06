﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Helpers;
using Ak.Framework.Core.Utils;
using Ak.Framework.Imaging.Extensions;
using Ak.Framework.Imaging.Helpers;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Culture;
using Ak.Framework.Wpf.Dialogs;
using Ak.Framework.Wpf.Helpers;
using Ak.Framework.Wpf.Messaging;
using Tira.App.Logic.Enums;
using Tira.App.Logic.Events;
using Tira.App.Logic.Helpers;
using Tira.App.Logic.ViewModels.Filters;
using Tira.App.Properties;
using Tira.App.Windows;
using Tira.App.Windows.Filters;
using Tira.Imaging.Classes;
using Tira.Imaging.Enums;
using Tira.Logic.Enums;
using Tira.Logic.Events;
using Tira.Logic.Helpers;
using Tira.Logic.Models;
using Tira.Logic.Models.Markup;
using Application = System.Windows.Application;
using DataColumn = Tira.Logic.Models.DataColumn;
using Message = Ak.Framework.Wpf.Messaging.Message;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class ProjectViewModel : TiraViewModelBase
    {
        #region Variables

        /// <summary>
        /// The need closing dialog
        /// </summary>
        private bool _needClosingDialog = true;

        /// <summary>
        /// Gallery images
        /// </summary>
        private BindingList<GalleryImage> _images = new BindingList<GalleryImage>();

        /// <summary>
        /// Current image
        /// </summary>
        private BitmapSource _selectedImage;

        /// <summary>
        /// Current image copy for work with filter
        /// </summary>
        private BitmapSource _selectedImageFilterCopy = null;

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

        /// <summary>
        /// Current image viewer mode
        /// </summary>
        private ImageViewerMode _currentImageViewerMode = ImageViewerMode.None;

        /// <summary>
        /// Current markup object type
        /// </summary>
        private MarkupObjectType _currentMarkupObjectType = MarkupObjectType.None;

        /// <summary>
        /// The copy of markup objects
        /// </summary>
        private MarkupObjects _copyOfMarkupObjects;

        /// <summary>
        /// Locker object
        /// </summary>
        private readonly object _lockerObject = new object();

        /// <summary>
        /// Flag for progress indicator activation
        /// </summary>
        private bool _progressIndicatorActivated;

        /// <summary>
        /// Long running operations data container
        /// </summary>
        private LongOperationsData _longOperationsDataContainer;

        /// <summary>
        /// Index of the selected data grid row
        /// </summary>
        private int? _selectedDataGridRowIndex;

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
                FillDataGrid();
                OnPropertyChanged(() => SelectedGalleryImage);
                OnPropertyChanged(() => CurrentMarkupObjects);
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

        /// <summary>
        /// Current image viewer mode
        /// </summary>
        public ImageViewerMode CurrentImageViewerMode
        {
            get => _currentImageViewerMode;
            set
            {
                _currentImageViewerMode = value;
                OnPropertyChanged(() => CurrentImageViewerMode);
            }
        }

        /// <summary>
        /// Current markup object type
        /// </summary>
        public MarkupObjectType CurrentMarkupObjectType
        {
            get => _currentMarkupObjectType;
            set
            {
                _currentMarkupObjectType = value;
                OnPropertyChanged(() => CurrentMarkupObjectType);
            }
        }

        /// <summary>
        /// Current markup objects
        /// </summary>
        public MarkupObjects CurrentMarkupObjects
        {
            get => SelectedGalleryImage?.MarkupObjects;
            set
            {
                if (SelectedGalleryImage != null)
                    SelectedGalleryImage.MarkupObjects = value;
                OnPropertyChanged(() => CurrentMarkupObjects);
            }
        }

        /// <summary>
        /// Flag for recognition completed
        /// </summary>
        public bool IsRecognitionCompleted
        {
            get => SelectedGalleryImage?.RecognitionCompleted ?? false;
            set
            {
                if (SelectedGalleryImage != null)
                    SelectedGalleryImage.RecognitionCompleted = value;
                OnPropertyChanged(() => IsRecognitionCompleted);
            }
        }

        /// <summary>
        /// Recognized data
        /// </summary>
        public DataTable RecognizedData
        {
            get => SelectedGalleryImage?.RecognizedData;                
            set
            {
                if (SelectedGalleryImage != null)
                    SelectedGalleryImage.RecognizedData = value;
                OnPropertyChanged(() => RecognizedData);
            }
        }

        /// <summary>
        /// Flag for progress indicator activation
        /// </summary>
        public bool ProgressIndicatorActivated
        {
            get => _progressIndicatorActivated;
            set
            {
                _progressIndicatorActivated = value;
                OnPropertyChanged(() => ProgressIndicatorActivated);
            }
        }

        /// <summary>
        /// Long running operations data container
        /// </summary>
        public LongOperationsData LongOperationsDataContainer
        {
            get => _longOperationsDataContainer;
            set
            {
                _longOperationsDataContainer = value;
                OnPropertyChanged(() => LongOperationsDataContainer);
            }
        }

        /// <summary>
        /// Index of the selected data grid row
        /// </summary>
        public int? SelectedDataGridRowIndex
        {
            get => _selectedDataGridRowIndex;
            set
            {
                _selectedDataGridRowIndex = value;
                OnPropertyChanged(() => SelectedDataGridRowIndex);
            }
        }

        #endregion

        #region Commands

        #region Project commands

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

        /// <summary>
        /// Command for handling window closing
        /// </summary>
        public INotifyCommand HandleWindowClosingCommand { get; }

        #endregion

        #region Templates commands

        /// <summary>
        /// Command for saving project as template
        /// </summary>
        public INotifyCommand SaveProjectAsTemplateCommand { get; }

        /// <summary>
        /// Command for showing templates window 
        /// </summary>
        public INotifyCommand ShowProjectTemplatesWindowCommand { get; }

        #endregion

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
        /// Command for image rollback one step
        /// </summary>
        public INotifyCommand ImageRollbackOneStepCommand { get; }

        /// <summary>
        /// Command for image zoom in
        /// </summary>
        public INotifyCommand ImageRollbackAllCommand { get; }

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

        /// <summary>
        /// Command for enabling image markup
        /// </summary>
        public INotifyCommand EnableMarkupCommand { get; }

        /// <summary>
        /// Command for clearing image markup
        /// </summary>
        public INotifyCommand ImageClearMarkupCommand { get; }

        /// <summary>
        /// Command for setting current markup object type
        /// </summary>
        public INotifyCommand SetCurrentMarkupObjectTypeCommand { get; }

        /// <summary>
        /// Command for handling markup objects changed command
        /// </summary>
        public INotifyCommand HandleMarkupObjectsChangedCommand { get; }

        /// <summary>
        /// Command for handling crop area selected command
        /// </summary>
        public INotifyCommand HandleCropAreaSelectedCommand { get; }

        /// <summary>
        /// Command for copying image markup
        /// </summary>
        public INotifyCommand ImageMarkupCopyCommand { get; }

        /// <summary>
        /// Command for pasting image markup
        /// </summary>
        public INotifyCommand ImageMarkupPasteCommand { get; }

        /// <summary>
        /// Command for image binarization
        /// </summary>
        public INotifyCommand ImageBinarizeCommand { get; }

        /// <summary>
        /// Command for image set brightness
        /// </summary>
        public INotifyCommand ImageSetBrightnessCommand { get; }

        /// <summary>
        /// Command for image set contrast
        /// </summary>
        public INotifyCommand ImageSetContrastCommand { get; }

        /// <summary>
        /// Command for image set gamma correction
        /// </summary>
        public INotifyCommand ImageSetGammaCorrectionCommand { get; }

        /// <summary>
        /// Command for image set sharpness
        /// </summary>
        public INotifyCommand ImageSetSharpnessCommand { get; }

        /// <summary>
        /// Command for image auto rotation
        /// </summary>
        public INotifyCommand ImageAutoRotateCommand { get; }

        /// <summary>
        /// Command for image rotation
        /// </summary>
        public INotifyCommand ImageRotateCommand { get; }

        /// <summary>
        /// Command for image auto resizing
        /// </summary>
        public INotifyCommand ImageAutoResizeCommand { get; }

        /// <summary>
        /// Command for image crop
        /// </summary>
        public INotifyCommand ImageCropCommand { get; }

        /// <summary>
        /// Command for image dilation
        /// </summary>
        public INotifyCommand ImageDilateCommand { get; }

        /// <summary>
        /// Command for image erosion
        /// </summary>
        public INotifyCommand ImageErodeCommand { get; }

        /// <summary>
        /// Command for removing lines from image
        /// </summary>
        public INotifyCommand ImageRemoveLinesCommand { get; }

        /// <summary>
        /// Command for removing noise from image
        /// </summary>
        public INotifyCommand ImageRemoveNoiseCommand { get; }

        /// <summary>
        /// Command for removing punch holes from image
        /// </summary>
        public INotifyCommand ImageRemovePunchHolesCommand { get; }

        /// <summary>
        /// Command for removing clip holes from image
        /// </summary>
        public INotifyCommand ImageRemoveStapleMarksCommand { get; }

        /// <summary>
        /// Command for removing blobs from image
        /// </summary>
        public INotifyCommand ImageRemoveBlobsCommand { get; }

        #endregion

        #region DataGrid commands

        /// <summary>
        /// Command for handling datagrid selected cell changed command
        /// </summary>
        public INotifyCommand HandleDataGridSelectedCellChangedCommand { get; }

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

            CreateProjectCommand = new NotifyCommand(o => CreateProject((Window)o));
            OpenProjectCommand = new NotifyCommand(o => OpenProject((Window)o));
            SaveProjectCommand = new NotifyCommand(_ => SaveProject());
            ExportProjectDataCommand = new NotifyCommand(o => ExportProjectData((ExportFormat)o));
            ShowSettingsWindowCommand = new NotifyCommand(_ => ShowSettingsWindow());            
            ShowAboutWindowCommand = new NotifyCommand(_ => ShowAboutWindow());
            CloseWindowCommand = new NotifyCommand(o => CloseWindow((Window)o));
            PerformOcrCommand = new NotifyCommand(_ => PerformOcr());
            ShowProjectSettingsWindowCommand = new NotifyCommand(_ => ShowProjectSettingsWindow());
            SaveProjectAsTemplateCommand = new NotifyCommand(_ => SaveProjectAsTemplate());
            ShowProjectTemplatesWindowCommand = new NotifyCommand(_ => ShowProjectTemplatesWindow());
            HandleWindowClosingCommand = new NotifyCommand(e => HandleWindowClosing((CancelEventArgs)e));

            AddImagesToGalleryCommand = new NotifyCommand(_ => AddImagesToGallery());
            RemoveImagesFromGalleryCommand = new NotifyCommand(_ => RemoveImagesFromGallery());
            HandleGalleryImageSelectedCommand = new NotifyCommand(e => FillGalleryImage((GalleryImageEventArgs)e));
            HandleGalleryImagesIdsSelectedCommand = new NotifyCommand(e => GetGalleryImagesUids((GalleryImagesUidsSelectionEventArgs)e));

            ImageRollbackOneStepCommand = new NotifyCommand(_ => ImageRollbackOneStep(), _ => CanPerformOperationsWithImage()); ;
            ImageRollbackAllCommand = new NotifyCommand(_ => ImageRollbackAll(), _ => CanPerformOperationsWithImage()); ;
            ImageZoomInCommand = new NotifyCommand(_ => ImageZoomIn(), _ => CanPerformOperationsWithImage());
            ImageZoomOutCommand = new NotifyCommand(_ => ImageZoomOut(), _ => CanPerformOperationsWithImage());
            ImageFitSizeCommand = new NotifyCommand(_ => ImageFitSize(), _ => CanPerformOperationsWithImage());
            ImageFitWidthCommand = new NotifyCommand(_ => ImageFitWidth(), _ => CanPerformOperationsWithImage());
            ImageFitHeightCommand = new NotifyCommand(_ => ImageFitHeight(), _ => CanPerformOperationsWithImage());
            EnableMarkupCommand = new NotifyCommand(o => ImageEnableMarkup((bool)o), _ => CanPerformOperationsWithImage());
            ImageClearMarkupCommand = new NotifyCommand(o => ImageClearMarkup(), _ => CanPerformOperationsWithImage());
            SetCurrentMarkupObjectTypeCommand = new NotifyCommand(o => SetCurrentMarkupObjectType((MarkupObjectType)o));
            HandleMarkupObjectsChangedCommand = new NotifyCommand(e => UpdateMarkupObjects((MarkupObjectsEventArgs)e));
            ImageMarkupCopyCommand = new NotifyCommand(_ => ImageMarkupCopy(), _ => CanPerformOperationsWithImage());
            ImageMarkupPasteCommand = new NotifyCommand(_ => ImageMarkupPaste(), _ => CanPerformOperationsWithImage());

            ImageBinarizeCommand = new NotifyCommand(_ => ImageBinarize(), _ => CanPerformOperationsWithImage());
            ImageSetBrightnessCommand = new NotifyCommand(_ => ImageSetBrightness(), _ => CanPerformOperationsWithImage());
            ImageSetContrastCommand = new NotifyCommand(_ => ImageSetContrast(), _ => CanPerformOperationsWithImage());
            ImageSetGammaCorrectionCommand = new NotifyCommand(_ => ImageSetGammaCorrection(), _ => CanPerformOperationsWithImage());
            ImageSetSharpnessCommand = new NotifyCommand(_ => ImageSetSharpness(), _ => CanPerformOperationsWithImage());
            ImageAutoRotateCommand = new NotifyCommand(_ => ImageAutoRotate(), _ => CanPerformOperationsWithImage());
            ImageRotateCommand = new NotifyCommand(_ => ImageRotate(), _ => CanPerformOperationsWithImage());
            ImageAutoResizeCommand = new NotifyCommand(_ => ImageAutoResize(), _ => CanPerformOperationsWithImage());
            ImageCropCommand = new NotifyCommand(_ => ImageCrop(), _ => CanPerformOperationsWithImage());
            ImageDilateCommand = new NotifyCommand(_ => ImageDilate(), _ => CanPerformOperationsWithImage());
            ImageErodeCommand = new NotifyCommand(_ => ImageErode(), _ => CanPerformOperationsWithImage());
            ImageRemoveLinesCommand = new NotifyCommand(_ => ImageRemoveLines(), _ => CanPerformOperationsWithImage());
            ImageRemoveNoiseCommand = new NotifyCommand(_ => ImageRemoveNoise(), _ => CanPerformOperationsWithImage());
            ImageRemovePunchHolesCommand = new NotifyCommand(_ => ImageRemovePunchHoles(), _ => CanPerformOperationsWithImage());
            ImageRemoveStapleMarksCommand = new NotifyCommand(_ => ImageRemoveStapleMarks(), _ => CanPerformOperationsWithImage());
            ImageRemoveBlobsCommand = new NotifyCommand(_ => ImageRemoveBlobs(), _ => CanPerformOperationsWithImage());
            HandleCropAreaSelectedCommand = new NotifyCommand(e => PerformImageCrop((RectangleAreaEventArgs)e), _ => CanPerformOperationsWithImage());

            HandleDataGridSelectedCellChangedCommand = new NotifyCommand(e => HighlightSelectedRow((DataGridCellInfo)e));

            Images.ListChanged += ImagesOnListChanged;
            Project.ProjectElementOcrCompleted += ProjectOnProjectElementOcrCompleted;

            // Messages from other viewmodels
            Messenger.Instance.Register(MessageType.SetFilterToSelectedImage, SetChangesToSelectedImage);
        }

        #endregion

        #region Private methods

        #region Project operations

        /// <summary>
        /// Creates the project
        /// </summary>
        /// <param name="window">Window</param>
        private void CreateProject(Window window)
        {
            try
            {
                ProjectCreationViewModel vm = new ProjectCreationViewModel();
                bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectCreationWindow>(vm);
                if (result.HasValue && result.Value)
                {
                    Project project = Project.Create(vm.ProjectPath, vm.Name, vm.SelectedProjectTemplate);
                    OpenProjectWindow(project, window);
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
        private void OpenProject(Window window)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = Project.ProjectFileExtensionsFilter };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Project project = Project.Load(openFileDialog.FileName);
                    OpenProjectWindow(project, window);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to open project");
                FormsHelper.ShowError(Resources.OpenProjectErrorMessage);
            }
        }

        /// <summary>
        /// Opens the project window and closes current
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="currentWindow">Current window</param>
        private void OpenProjectWindow(Project project, Window currentWindow)
        {
            _needClosingDialog = false;
            currentWindow.Hide();
            new ProjectWindow(new ProjectViewModel(project)).Show();
            currentWindow.Close();
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
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                {
                    Project.PerformOcr();
                };
                worker.RunWorkerCompleted += (sender, args) =>
                {
                    ProgressIndicatorActivated = false;
                    if (result.Result == ActionResultType.Ok)
                    {
                        FormsHelper.ShowMessage(Resources.ProjectWindow_OcrCompletedMessage_Text, Resources.ProjectCreationWindow_ProjectValidationMessage_Caption);                        
                        FillDataGrid();
                    }
                    else
                        FormsHelper.ShowUnexpectedError();
                };
                ProgressIndicatorActivated = true;
                worker.RunWorkerAsync();
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
                        FileName = $"{Project.Name}.{ExportFormatsHelper.GetFileExtension(exportFormat)}",
                        Filter = ExportFormatsHelper.GetFileFilterForSaveDialog(exportFormat)
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
                    Project.UpdateDataColumns(new List<DataColumn>(vm.DataColumns));
                    RecognizedData = null;
                    SelectedDataGridRowIndex = null;
                    IsRecognitionCompleted = false;
                    FillDataGridColumns();

                    if (CurrentMarkupObjects != null)
                    {
                        CurrentMarkupObjects.MaxNumberOfVerticalLines = vm.DataColumns.Count - 1;
                        OnPropertyChanged(() => CurrentMarkupObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Unable to show and save project settings.");
                FormsHelper.ShowUnexpectedError();
            }
        }

        /// <summary>
        /// Handles window closing
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void HandleWindowClosing(CancelEventArgs e)
        {
            if (_needClosingDialog)
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(Resources.ProjectWindowClosing_Text, Resources.ProjectWindowClosing_Caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;

                    case MessageBoxResult.Yes:
                        Project.Save();
                        break;
                }
            }
        }

        #endregion

        #region Templates operations

        /// <summary>
        /// Saves project as template
        /// </summary>
        private void SaveProjectAsTemplate()
        {
            ProjectTemplateViewModel vm = new ProjectTemplateViewModel(WindowMode.Create);
            bool? result = ShowDialogAgent.Instance.ShowDialog<ProjectTemplateWindow>(vm);
            if (result.HasValue && result.Value)
                Project.SaveAsTemplate(vm.Name);
        }

        /// <summary>
        /// Shows templates window
        /// </summary>
        private void ShowProjectTemplatesWindow()
        {
            ShowDialogAgent.Instance.ShowDialog<ProjectTemplatesWindow>(new ProjectTemplatesViewModel());
        }

        #endregion

        #region Gallery operations

        /// <summary>
        /// Fills the gallery control
        /// </summary>
        public void FillGallery()
        {
            _copyOfMarkupObjects = null;

            _images = new BindingList<GalleryImage>();
            if (Project.Gallery.Images.Count > 0)
                foreach (GalleryImage galleryImage in Project.Gallery.Images)
                    _images.Add(galleryImage);

            OnPropertyChanged(() => Images);
        }

        /// <summary>
        /// Adds images to gallery
        /// </summary>
        private void AddImagesToGallery()
        {
            OpenFileDialog imagesImportFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = string.Join("|", Gallery.ImageFilesExtensionsFilter) 
            };
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
                Project.Gallery.Remove(SelectedImagesUids);
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
        /// Replaces thumbnail in gallery
        /// </summary>
        /// <param name="galleryImage">Gallery image</param>
        private void ReplaceGalleryThumbnail(GalleryImage galleryImage)
        {
            GalleryImage image = _images.WhereEx(x => x.Uid == galleryImage.Uid).FirstOrDefault();
            if (image != null)
            {
                image.TempFilePath = galleryImage.TempFilePath;
                image.TempThumbnailFilePath = galleryImage.TempThumbnailFilePath;
                OnPropertyChanged(() => Images);
            }
        }

        #endregion

        #region Image operations

        /// <summary>
        /// Fills the gallery image.
        /// </summary>
        /// <param name="args">The <see cref="GalleryImageEventArgs"/> instance containing the event data.</param>
        private void FillGalleryImage(GalleryImageEventArgs args)
        {
            if (SelectedGalleryImage == null || args.Image.Uid != SelectedGalleryImage.Uid)
            {
                _selectedImageFilterCopy = null;
                ImageViewerZoomManager = new ZoomManager();
                OnPropertyChanged(() => ImageViewerZoomManager);

                SelectedGalleryImage = args.Image;
                CurrentMarkupObjects = SelectedGalleryImage.MarkupObjects;
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
                OnPropertyChanged(() => CurrentMarkupObjects);

                ImageLoadedToViewer = true;
            }
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

        /// <summary>
        /// Enable markup for image
        /// </summary>
        /// <param name="isMarkupModeEnabled">Flag for checking if markup mode enabled</param>
        private void ImageEnableMarkup(bool isMarkupModeEnabled)
        {
            CurrentImageViewerMode = isMarkupModeEnabled ? ImageViewerMode.Markup : ImageViewerMode.None;
            CurrentMarkupObjectType = MarkupObjectType.None;
        }

        /// <summary>
        /// Clears image markup
        /// </summary>
        private void ImageClearMarkup()
        {
            CurrentMarkupObjectType = MarkupObjectType.None;
            SelectedGalleryImage.MarkupObjects = new MarkupObjects(Project.DataColumns.Count - 1);
            IsRecognitionCompleted = false;
            RecognizedData = null;
            OnPropertyChanged(() => SelectedGalleryImage);
            OnPropertyChanged(() => CurrentMarkupObjects);
        }

        /// <summary>
        /// Copy image markup
        /// </summary>
        private void ImageMarkupCopy()
        {
            if (SelectedGalleryImage != null)
                _copyOfMarkupObjects = SelectedGalleryImage.MarkupObjects.Clone();
        }

        /// <summary>
        /// Paste image markup
        /// </summary>
        private void ImageMarkupPaste()
        {
            if (SelectedGalleryImage != null && _copyOfMarkupObjects != null)
            {
                GalleryImage image = Project.Gallery.Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
                if (image != null)
                {
                    image.MarkupObjects = _copyOfMarkupObjects.Clone();
                    SelectedGalleryImage.MarkupObjects = _copyOfMarkupObjects.Clone();
                    OnPropertyChanged(() => SelectedGalleryImage);
                    OnPropertyChanged(() => CurrentMarkupObjects);
                }
            }
        }

        /// <summary>
        /// Roll image one step back
        /// </summary>
        private void ImageRollbackOneStep()
        {
            _selectedImageFilterCopy = null;

            GalleryImage galleryImage = Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
            if (galleryImage != null && galleryImage.AppliedFilters.Count > 0)
            {
                bool needRemoveMarkupObjects = Filter.RemoveMarkupObjects(galleryImage.AppliedFilters.Last().FilterType);
                galleryImage.RollBackFiltersOneStepBack();
                SelectedGalleryImage = galleryImage;
                ReplaceGalleryThumbnail(SelectedGalleryImage);

                if (needRemoveMarkupObjects)
                {
                    galleryImage.MarkupObjects = new MarkupObjects(Project.DataColumns.Count - 1);
                    ImageClearMarkup();
                }

                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
            }
        }

        /// <summary>
        /// Restore original image
        /// </summary>
        private void ImageRollbackAll()
        {
            _selectedImageFilterCopy = null;

            GalleryImage galleryImage = Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
            if (galleryImage != null && galleryImage.AppliedFilters.Count > 0)
            {
                bool needRemoveMarkupObjects = Filter.CheckExistanceOfFilterWithMarkupObjectsRemoval(galleryImage.AppliedFilters);
                galleryImage.RollBackToOriginal();
                SelectedGalleryImage = galleryImage;
                ReplaceGalleryThumbnail(SelectedGalleryImage);

                if (needRemoveMarkupObjects)
                {
                    galleryImage.MarkupObjects = new MarkupObjects(Project.DataColumns.Count - 1);
                    ImageClearMarkup();
                }

                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
            }
        }

        /// <summary>
        /// Binarize image
        /// </summary>
        private void ImageBinarize()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.Binarization, 128);
            bool? result = ShowDialogAgent.Instance.ShowDialog<BinarizationFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Binarization, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Set brightness for image
        /// </summary>
        private void ImageSetBrightness()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.Brightness, 0);
            bool? result = ShowDialogAgent.Instance.ShowDialog<BrightnessFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Brightness, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Set contrast for image
        /// </summary>
        private void ImageSetContrast()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.Contrast, 0);
            bool? result = ShowDialogAgent.Instance.ShowDialog<ContrastFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Contrast, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Set sharpness for image
        /// </summary>
        private void ImageSetSharpness()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.Sharpness, 0);
            bool? result = ShowDialogAgent.Instance.ShowDialog<SharpnessFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Sharpness, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Set gamma correction for image
        /// </summary>
        private void ImageSetGammaCorrection()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.GammaCorrection, 0);
            bool? result = ShowDialogAgent.Instance.ShowDialog<GammaCorrectonFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.GammaCorrection, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Auto rotate image
        /// </summary>
        private void ImageAutoRotate()
        {
            ApplyFilter(new Filter(FilterType.AutoDeskew, new AutoDeskewParameters()));
        }

        /// <summary>
        /// Rotate image
        /// </summary>
        private void ImageRotate()
        {
            IntValueFilterViewModel vm = new IntValueFilterViewModel(FilterType.Rotation, 0);
            bool? result = ShowDialogAgent.Instance.ShowDialog<RotationFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Rotation, vm.IntValue));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Auto resize image
        /// </summary>
        private void ImageAutoResize()
        {
            ApplyFilter(new Filter(FilterType.AutoCrop, null));
        }

        /// <summary>
        /// Crop image
        /// </summary>
        private void ImageCrop()
        {
            CurrentImageViewerMode = ImageViewerMode.Crop;
            CurrentMarkupObjectType = MarkupObjectType.None;
        }

        /// <summary>
        /// Performs image cropping
        /// </summary>
        /// <param name="e">The <see cref="Tira.App.Logic.Events.RectangleAreaEventArgs" /> instance containing the event data.</param>
        private void PerformImageCrop(RectangleAreaEventArgs e)
        {
            bool? result = ShowDialogAgent.Instance.ShowDialog<CropFilterWindow>(null);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.Crop, e.Rectangle));

            CurrentImageViewerMode = ImageViewerMode.None;
        }

        /// <summary>
        /// Dilate image
        /// </summary>
        private void ImageDilate()
        {
            ApplyFilter(new Filter(FilterType.Dilation, null));
        }

        /// <summary>
        /// Erode image
        /// </summary>
        private void ImageErode()
        {
            ApplyFilter(new Filter(FilterType.Erosion, null));
        }

        /// <summary>
        /// Remove lines from image
        /// </summary>
        private void ImageRemoveLines()
        {
            LinesRemovalFilterViewModel vm = new LinesRemovalFilterViewModel();
            bool? result = ShowDialogAgent.Instance.ShowDialog<LinesRemovalFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.LinesRemoval, vm.LineRemoveOrientation));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Remove noise from image
        /// </summary>
        private void ImageRemoveNoise()
        {
            ApplyFilter(new Filter(FilterType.NoiseRemoval, null));
        }

        /// <summary>
        /// Remove punch holes from image
        /// </summary>
        private void ImageRemovePunchHoles()
        {
            PunchHolesRemovalFilterViewModel vm = new PunchHolesRemovalFilterViewModel();
            bool? result = ShowDialogAgent.Instance.ShowDialog<PunchHolesRemovalFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.PunchHolesRemoval, vm.PunchHolesPositions));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Remove clip holes from image
        /// </summary>
        private void ImageRemoveStapleMarks()
        {
            ApplyFilter(new Filter(FilterType.StapleMarksRemoval, null));
        }

        /// <summary>
        /// Remove blobs from image
        /// </summary>
        private void ImageRemoveBlobs()
        {
            BlobsRemovalFilterViewModel vm = new BlobsRemovalFilterViewModel();
            bool? result = ShowDialogAgent.Instance.ShowDialog<BlobsRemovalFilterWindow>(vm);
            if (result.HasValue && result.Value)
                ApplyFilter(new Filter(FilterType.BlobsRemoval, vm.BlobsDimensions));
            else
                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
        }

        /// <summary>
        /// Sets current markup object type
        /// </summary>
        /// <param name="markupObjectType">Markup object type</param>
        private void SetCurrentMarkupObjectType(MarkupObjectType markupObjectType)
        {
            if (CurrentMarkupObjects == null)
                return;

            switch (markupObjectType)
            {
                case MarkupObjectType.None:
                case MarkupObjectType.Rectangle:
                    break;

                case MarkupObjectType.VerticalLine:
                    if (DataGridColumns.Count < 2)
                    {
                        FormsHelper.ShowWarning(Resources.NotEnoughDataGridColumnsInMarkupWarning_Text, Resources.NotEnoughDataGridColumnsInMarkupWarning_Caption);
                        return;   
                    }
                    if (CurrentMarkupObjects.RectangleArea == Rectangle.Empty)
                    {
                        FormsHelper.ShowWarning(Resources.NoRectangleInMarkupWarning_Text, Resources.NoRectangleInMarkupWarning_Caption);
                        return;
                    }
                    break;

                case MarkupObjectType.HorizontalLine:
                    if (CurrentMarkupObjects.RectangleArea == Rectangle.Empty)
                    {
                        FormsHelper.ShowWarning(Resources.NoRectangleInMarkupWarning_Text, Resources.NoRectangleInMarkupWarning_Caption);
                        return;
                    }
                    break;
            }

            CurrentMarkupObjectType = markupObjectType;
        }

        /// <summary>
        /// Updates markup objects
        /// </summary>
        private void UpdateMarkupObjects(MarkupObjectsEventArgs e)
        {
            if (SelectedGalleryImage != null)
            {
                GalleryImage image = Project.Gallery.Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
                if (image != null)
                    image.MarkupObjects = e.MarkupObjects;
            }
        }

        /// <summary>
        /// Applies filter.
        /// </summary>
        /// <param name="filter">Filter</param>
        private void ApplyFilter(Filter filter)
        {
            _selectedImageFilterCopy = null;

            GalleryImage galleryImage = Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
            if (galleryImage != null)
            {
                galleryImage.ApplyFilter(filter);
                SelectedGalleryImage = galleryImage;
                ReplaceGalleryThumbnail(SelectedGalleryImage);

                if (Filter.RemoveMarkupObjects(filter.FilterType))
                {
                    galleryImage.MarkupObjects = new MarkupObjects(Project.DataColumns.Count - 1);
                    ImageClearMarkup();
                }

                SelectedImage = BitmapImageHelper.GetBitmapImageFromPath(SelectedGalleryImage.ActualImageFilePath);
            }
        }

        #region Selected image changes

        /// <summary>
        /// Sets changes to selected image
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="args">Arguments</param>
        private void SetChangesToSelectedImage(Message message, params object[] args)
        {
            lock (_lockerObject)
            {
                GarbageCollector.Collect();

                if (_selectedImageFilterCopy == null)
                    _selectedImageFilterCopy = SelectedImage;

                switch ((FilterType)args[0])
                {
                    case FilterType.Binarization:
                        SelectedImage = Imaging.Helpers.BitmapHelper.Binarize(_selectedImageFilterCopy.ToBitmap(), (byte)args[1].ToInt32()).ToBitmapSource();
                        break;

                    case FilterType.Brightness:
                        SelectedImage = Imaging.Helpers.BitmapHelper.SetBrightness(_selectedImageFilterCopy.ToBitmap(), args[1].ToInt32()).ToBitmapSource();
                        break;

                    case FilterType.Contrast:
                        SelectedImage = Imaging.Helpers.BitmapHelper.SetContrast(_selectedImageFilterCopy.ToBitmap(), args[1].ToInt32()).ToBitmapSource();
                        break;

                    case FilterType.GammaCorrection:
                        SelectedImage = Imaging.Helpers.BitmapHelper.SetGammaCorrection(_selectedImageFilterCopy.ToBitmap(), args[1].ToInt32()).ToBitmapSource();
                        break;

                    case FilterType.Sharpness:
                        SelectedImage = Imaging.Helpers.BitmapHelper.SetSharpness(_selectedImageFilterCopy.ToBitmap(), args[1].ToInt32()).ToBitmapSource();
                        break;

                    case FilterType.Rotation:
                        SelectedImage = Imaging.Helpers.BitmapHelper.Rotate(_selectedImageFilterCopy.ToBitmap(), (float)args[1].ToInt32(), Color.Black).ToBitmapSource();
                        break;

                    case FilterType.BlobsRemoval:
                        SelectedImage = Imaging.Helpers.BitmapHelper.RemoveBlobs(_selectedImageFilterCopy.ToBitmap(), (BlobsDimensions)args[1]).ToBitmapSource();
                        break;

                    case FilterType.PunchHolesRemoval:
                        SelectedImage = Imaging.Helpers.BitmapHelper.RemovePunchHoles(_selectedImageFilterCopy.ToBitmap(), (PunchHolesPositions)args[1]).ToBitmapSource();
                        break;

                    case FilterType.LinesRemoval:
                        SelectedImage = Imaging.Helpers.BitmapHelper.RemoveLines(_selectedImageFilterCopy.ToBitmap(), args[1].ToEnum(LineRemoveOrientation.None)).ToBitmapSource();
                        break;
                }
            }

            OnPropertyChanged(() => ImageViewerZoomManager);
        }

        #endregion

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

        /// <summary>
        /// Fills the data grid
        /// </summary>
        private void FillDataGrid()
        {
            SelectedDataGridRowIndex = null;
            FillDataGridColumns();

            if (SelectedGalleryImage != null && SelectedGalleryImage.RecognitionCompleted && SelectedGalleryImage.MarkupObjects.MaxNumberOfVerticalLines == SelectedGalleryImage.RecognizedData.Columns.Count - 1)
            {
                GalleryImage galleryImage = Project.Gallery.Images.WhereEx(x => x.Uid == SelectedGalleryImage.Uid).FirstOrDefault();
                IsRecognitionCompleted = galleryImage.RecognitionCompleted;
                RecognizedData = galleryImage.RecognizedData;                
            }                            
        }

        /// <summary>
        /// Highlights selected row on image
        /// </summary>
        /// <param name="e">Cell info</param>
        private void HighlightSelectedRow(DataGridCellInfo e)
        {
            try
            {
                System.Windows.Controls.DataGridCell c = DataGridHelper.GetCell(e);
                if (c != null)
                    SelectedDataGridRowIndex = DataGridHelper.GetRowIndex(c);
            }
            catch
            {
            }
        }

        #endregion

        #endregion

        #region Events handlers

        private void ImagesOnListChanged(object sender, ListChangedEventArgs listChangedEventArgs)
        {
            if (listChangedEventArgs.ListChangedType == ListChangedType.ItemAdded)
                Project.Gallery.Sort(Images.Select(x => x.Uid));
        }

        private void ProjectOnProjectElementOcrCompleted(object sender, LongOperationsDataEventArgs longOperationsDataEventArgs)
        {            
            Application.Current.Dispatcher.Invoke(() => LongOperationsDataContainer = longOperationsDataEventArgs.LongOperationsData, DispatcherPriority.Background);
        }

        #endregion
    }
}