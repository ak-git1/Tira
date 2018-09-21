using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Ak.Framework.Core.Helpers;
using Tira.Logic.Engines;
using Tira.Logic.Enums;
using Tira.Logic.Events;
using Tira.Logic.Helpers;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Recoginition project
    /// </summary>
    [Serializable]
    public class Project
    {
        #region Variables and constants

        /// <summary>
        /// The data folder prefix
        /// </summary>
        private const string DataFolderPrefix = "data";

        /// <summary>
        /// The project file extensions filter
        /// </summary>
        public const string ProjectFileExtensionsFilter = "Project files (*.rproj)|*.rproj";

        /// <summary>
        /// Container for long operations data
        /// </summary>
        private LongOperationsData _longOperationsDataContainer;

        #endregion

        #region Properties

        /// <summary>
        /// Application version
        /// </summary>
        [XmlElement]
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Project name
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Path to ptoject file
        /// </summary>
        [XmlElement]
        public string ProjectPath { get; private set; }

        /// <summary>
        /// Path to the data folder
        /// </summary>
        [XmlElement]
        public string ProjectDataFolderPath { get; set; }

        /// <summary>
        /// Gallery
        /// </summary>
        [XmlElement]
        public Gallery Gallery { get; set; }

        /// <summary>
        /// Data columns
        /// </summary>
        [XmlElement]
        public List<DataColumn> DataColumns { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event fired when project element ocr completed
        /// </summary>
        [field: NonSerialized]
        public event ProjectElementOcrCompletedEventHandler ProjectElementOcrCompleted;

        #endregion

        #region Delegates

        /// <summary>
        /// Delefate for event fired when project element ocr completed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Tira.Logic.Events.LongOperationsDataEventArgs" /> instance containing the event data.</param>
        public delegate void ProjectElementOcrCompletedEventHandler(object sender, LongOperationsDataEventArgs e);

        #endregion

        #region Public methods

        /// <summary>
        /// Project creation
        /// </summary>
        /// <param name="projectPath">Project path</param>
        /// <param name="name">Name</param>
        /// <param name="projectTemplate">Project template</param>
        /// <returns></returns>
        public static Project Create(string projectPath, string name, ProjectTemplate projectTemplate)
        {
            Project project = new Project
            {
                ApplicationVersion = AssemblyInfoHelper.GetMainAssemblyVersion(),
                ProjectPath = projectPath,
                Name = name,
                ProjectDataFolderPath = Path.Combine(Path.GetDirectoryName(projectPath), $@"{DataFolderPrefix}")
            };
            project.Gallery = new Gallery(project.ProjectDataFolderPath);
            project.DataColumns = projectTemplate == null ? 
                new List<DataColumn>()
                : SerializationHelper.DeserializeFromXml<List<DataColumn>>(projectTemplate.Data);
            project.Save();

            // Adding project to recent projects list
            new RecentProject(project.Name, project.ProjectPath).AddOrUpdate();

            // Deleting temporary files
            project.Gallery.DeleteTempFiles();

            return project;
        }

        /// <summary>
        /// Loads project from the specified project path.
        /// </summary>
        /// <param name="projectPath">The project path.</param>
        /// <returns></returns>
        public static Project Load(string projectPath)
        {
            Project project = SerializationHelper.DeserializeFromXml<Project>(File.ReadAllText(projectPath));
            project.WireUpEvents();
            project.UpdateProjectPathes(projectPath);

            // Adding project to recent projects list
            new RecentProject(project.Name, project.ProjectPath).AddOrUpdate();

            // Deleting temporary files
            project.Gallery.DeleteTempFiles();

            return project;
        }

        /// <summary>
        /// Saves project
        /// </summary>
        public void Save()
        {
            ApplicationVersion = AssemblyInfoHelper.GetMainAssemblyVersion();
            Directory.CreateDirectory(ProjectDataFolderPath);
            Gallery.Save();
            File.WriteAllText(ProjectPath, SerializationHelper.SerializeToXml(this));
        }

        /// <summary>
        /// Saves project to new location
        /// </summary>
        public void SaveAs()
        {
            // TODO
        }

        /// <summary>
        /// Updates the data columns.
        /// </summary>
        /// <param name="dataColumns">Data columns.</param>
        public void UpdateDataColumns(List<DataColumn> dataColumns)
        {
            DataColumns = dataColumns;
            int maxNumberOfVerticalLines = DataColumns.Count - 1;
            foreach (GalleryImage image in Gallery.Images)
            {
                image.MarkupObjects.MaxNumberOfVerticalLines = maxNumberOfVerticalLines;
                image.RecognitionCompleted = false;
                image.RecognizedData = null;
            }
        }

        /// <summary>
        /// Validates OCR possibility
        /// </summary>
        public ActionResult ValidateOcr()
        {
            if (DataColumns.Count == 0)
                return new ActionResult(ActionResultType.Error, Properties.Resources.ValidateOcr_Messages_NoDataColumns);

            if (Gallery.Images.Count == 0)
                return new ActionResult(ActionResultType.Error, Properties.Resources.ValidateOcr_Messages_NoImages);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (GalleryImage image in Gallery.Images)
            {
                MarkupObjectsValidationResult markupObjectsValidationResult = image.MarkupObjects.Validate();
                switch (markupObjectsValidationResult)
                {
                    case MarkupObjectsValidationResult.RectangleNotSet:
                        stringBuilder.AppendLine(string.Format(Properties.Resources.ValidateOcr_Messages_RectangleNotSet, image.OrderNumber));
                        break;

                    case MarkupObjectsValidationResult.WrongNumberOfVerticalLines:
                        stringBuilder.AppendLine(string.Format(Properties.Resources.ValidateOcr_Messages_WrongNumberOfVerticalLines, image.OrderNumber));
                        break;
                }
            }
            if (stringBuilder.Length > 0)
                return new ActionResult(ActionResultType.Error, stringBuilder.ToString());

            return new ActionResult();
        }

        /// <summary>
        /// Performs OCR on project images.
        /// </summary>
        /// <returns></returns>
        public ActionResult PerformOcr()
        {
            Gallery.RecognizableElementOcrCompleted += Gallery_RecognizableElementOcrCompleted;
            _longOperationsDataContainer = new LongOperationsData(0, Gallery.GetRecognizableElementsQuantity(), Properties.Resources.PerformOcr_CurrentIterationMessageTemplate, Properties.Resources.PerformOcr_Description);                        
            ProjectElementOcrCompleted?.Invoke(this, new LongOperationsDataEventArgs(_longOperationsDataContainer));

            try
            {
                foreach (GalleryImage image in Gallery.Images)
                    image.PerformOcr(DataColumns);                

                return new ActionResult();
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e, $"Unable to perform OCR in the project '{Name}'");
                return new ActionResult(ActionResultType.Error, e.Message);
            }
            finally
            {
                _longOperationsDataContainer = null;
                Gallery.RecognizableElementOcrCompleted -= Gallery_RecognizableElementOcrCompleted;
            }
        }

        /// <summary>
        /// Exports recognized data to specific format
        /// </summary>
        /// <param name="path">Export file path</param>
        /// <param name="exportFormat">Export format</param>
        public void Export(string path, ExportFormat exportFormat)
        {
            ExportFilesEnginesFactory.GetExportFilesEngine(exportFormat).GenerateFile(GetMergedRecognizedData(), path);
        }        

        /// <summary>
        /// Check if recognition export allowed
        /// </summary>
        public ActionResult CheckRecognitionResultExportAllowed()
        {
            foreach (GalleryImage image in Gallery.Images)
                if (!image.RecognitionCompleted)
                    return new ActionResult(ActionResultType.Error, string.Format(Properties.Resources.CheckRecognitionResultExportAllowed_ErrorMessage, image.OrderNumber));

            return new ActionResult();
        }

        /// <summary>
        /// Saves project as template
        /// </summary>
        /// <param name="name">Template name</param>
        public void SaveAsTemplate(string name)
        {
            ProjectTemplate projectTemplate = new ProjectTemplate(name, SerializationHelper.SerializeToXml(DataColumns));
            projectTemplate.Add();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Wires up events
        /// </summary>
        private void WireUpEvents()
        {
            Gallery.WireUpEvents();
            Gallery.RecognizableElementOcrCompleted += Gallery_RecognizableElementOcrCompleted;            
        }

        /// <summary>
        /// Updates project pathes.
        /// </summary>
        /// <param name="projectPath">The project path.</param>
        private void UpdateProjectPathes(string projectPath)
        {
            ProjectPath = projectPath;
            ProjectDataFolderPath = Path.Combine(Path.GetDirectoryName(projectPath), Path.GetFileName(Path.GetDirectoryName(ProjectDataFolderPath + @"\")));
            Gallery.UpdateGalleryPathes(ProjectDataFolderPath);
        }

        /// <summary>
        /// Gets merged recognized data from all images
        /// </summary>
        /// <returns></returns>
        private DataTable GetMergedRecognizedData()
        {
            DataTable dataTable = Gallery.Images.First().RecognizedData.Copy();
            if (Gallery.Images.Count > 1)
                for (int i = 1; i < Gallery.Images.Count; i++)
                    dataTable.Merge(Gallery.Images[i].RecognizedData.Copy());

            return dataTable;
        }

        #endregion

        #region Events handlers

        private void Gallery_RecognizableElementOcrCompleted(object sender, EventArgs eventArgs)
        {
            if (_longOperationsDataContainer != null)
            {
                _longOperationsDataContainer.Iterate();
                ProjectElementOcrCompleted?.Invoke(this, new LongOperationsDataEventArgs(_longOperationsDataContainer));
            }
        }

        #endregion
    }
}