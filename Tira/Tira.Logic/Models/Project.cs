using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Tira.Logic.Enums;
using Tira.Logic.Helpers;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Recoginition project
    /// </summary>
    [Serializable]
    internal class Project
    {
        #region Variables and constants

        /// <summary>
        /// The data folder prefix
        /// </summary>
        private const string DataFolderPrefix = "data";

        /// <summary>
        /// The project file extensions filter
        /// </summary>
        public const string ProjectFileExtensionsFilter = "*.rproj|*.rproj";

        #endregion

        #region Properties

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
        public List<string> DataColumns { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Project creation
        /// </summary>
        /// <returns></returns>
        public static Project Create(string projectPath, string name)
        {
            Project project = new Project
            {
                ProjectPath = projectPath,
                Name = name,
                ProjectDataFolderPath = Path.Combine(Path.GetDirectoryName(projectPath), $@"{DataFolderPrefix}")
            };
            project.Gallery = new Gallery(project.ProjectDataFolderPath);
            project.DataColumns = new List<string>();
            project.Save();
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
            project.UpdateProjectPathes(projectPath);
            return project;
        }

        /// <summary>
        /// Saves project
        /// </summary>
        public void Save()
        {
            Directory.CreateDirectory(ProjectDataFolderPath);
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
        public void UpdateDataColumns(List<string> dataColumns)
        {
            DataColumns = dataColumns;
            int maxNumberOfVerticalLines = DataColumns.Count - 1;
            foreach (GalleryImage image in Gallery.Images)
                image.DrawingObjects.MaxNumberOfVerticalLines = maxNumberOfVerticalLines;
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
                DrawingObjectsValidationResult drawingObjectsValidationResult = image.DrawingObjects.Validate();
                switch (drawingObjectsValidationResult)
                {
                    case DrawingObjectsValidationResult.RectangleNotSet:
                        stringBuilder.AppendLine(string.Format(Properties.Resources.ValidateOcr_Messages_RectangleNotSet, image.OrderNumber));
                        break;

                    case DrawingObjectsValidationResult.WrongNumberOfVerticalLines:
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
        }

        /// <summary>
        /// Exports recognized data to json format
        /// </summary>
        /// <param name="path">Export file path.</param>
        public void ExportJson(string path)
        {
            DataTable dataTable = Gallery.Images.First().RecognizedData.Copy();
            if (Gallery.Images.Count > 1)
                for (int i = 1; i < Gallery.Images.Count; i++)
                    dataTable.Merge(Gallery.Images[i].RecognizedData.Copy());

            string json = JsonConvert.SerializeObject(dataTable);
            File.WriteAllText(path, json);
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

        #endregion

        #region Private methods

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
      
        #endregion
    }
}