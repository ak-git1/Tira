using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Tira.Imaging.Converters;
using Tira.Imaging.Engines;
using Tira.Imaging.Interfaces;
using Tira.Logic.Enums;
using Tira.Logic.Helpers;
using Tira.Logic.Models.Markup;
using Tira.Logic.Settings;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Images gallery
    /// </summary>
    [Serializable]
    public class Gallery
    {
        #region Variables and constants

        /// <summary>
        /// The image name mask
        /// </summary>
        private const string ImageNameMask = "0000000";

        /// <summary>
        /// The image files extensions filter
        /// </summary>
        public static List<string> ImageFilesExtensionsFilter = new List<string>
        {
            "All images files (*.jpeg; *.jpg; *.png; *.gif; *.bmp; *.pdf; *.tif; *.tiff)|*.jpeg;*.jpg;*.png;*.gif;*.bmp;*.pdf;*.tif; *.tiff",
            "Jpeg (*.jpeg; *.jpg)|*.jpeg;*.jpg",
            "Png (*.png)|*.png",
            "Gif (*.gif)|*.gif",
            "Bmp (*.bmp)|*.bmp",
            "Pdf (*.pdf)|*.pdf",
            "Tiff (*.tif; *.tiff)|*.tif; *.tiff",
        };

        #endregion

        #region Properties

        /// <summary>
        /// Gallery folder path
        /// </summary>
        [XmlElement]
        public string GalleryFolderPath { get; set; }

        /// <summary>
        /// Gallery
        /// </summary>
        [XmlElement]
        public List<GalleryImage> Images { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event fired when element recognigtion completed
        /// </summary>
        [field: NonSerialized]
        public EventHandler RecognizableElementOcrCompleted;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gallery"/> class.
        /// </summary>
        /// <param name="galleryFolderPath">The gallery folder path.</param>
        public Gallery(string galleryFolderPath)
        {
            GalleryFolderPath = galleryFolderPath;
            Images = new List<GalleryImage>();            
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds files to gallery
        /// </summary>
        /// <param name="files">The files</param>
        /// <param name="maxNumberOfVerticalLines">The maximum number of vertical lines.</param>
        /// <returns></returns>
        public List<ActionResult> AddFiles(string[] files, int maxNumberOfVerticalLines = 1)
        {
            List<ActionResult> results = new List<ActionResult>();

            foreach (string file in files)
                results.Add(AddFile(file, maxNumberOfVerticalLines));

            return results;
        }

        /// <summary>
        /// Removes images for specified
        /// </summary>
        /// <param name="selectedImagesUids">The selected images uids.</param>
        public void Remove(List<Guid> selectedImagesUids)
        {
            Images.RemoveAll(r => selectedImagesUids.Contains(r.Uid));
        }

        /// <summary>
        /// Updates gallery pathes.
        /// </summary>
        /// <param name="galleryFolderPath">The gallery path.</param>
        public void UpdateGalleryPathes(string galleryFolderPath)
        {            
            GalleryFolderPath = galleryFolderPath;
            foreach (GalleryImage image in Images)
                image.ImageFolderPath = galleryFolderPath;
        }

        /// <summary>
        /// Sorts by uids list
        /// </summary>
        public void Sort(IEnumerable<Guid> uids)
        {
            List<GalleryImage> list = new List<GalleryImage>();
            foreach (Guid uid in uids)
                list.Add(Images.First(x => x.Uid == uid));

            Images = list;
        }

        /// <summary>
        /// Saves gallery
        /// </summary>
        public void Save()
        {
            foreach (GalleryImage galleryImage in Images)
                galleryImage.Save();

            DeleteTempFiles();
        }

        /// <summary>
        /// Deletes temporary files
        /// </summary>
        public void DeleteTempFiles()
        {
            string[] files = Directory.GetFiles(GalleryFolderPath);
            List<string> galleryFiles = new List<string>();
            foreach (GalleryImage image in Images)
            {
                galleryFiles.Add(image.ImageFilePath);
                galleryFiles.Add(image.ThumbnailFilePath);
                galleryFiles.Add(image.OriginalFilePath);

                image.TempFilePath = string.Empty;
                image.TempThumbnailFilePath = string.Empty;
            }

            foreach (string file in files)
                if (!galleryFiles.Contains(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(e, $"Unable to delete temporary file: {file}");
                    }
                }                        
        }

        /// <summary>
        /// Gets quantity of all recognizable elements on all images
        /// </summary>
        /// <returns></returns>
        public int GetRecognizableElementsQuantity()
        {
            int counter = 0;
            foreach (GalleryImage image in Images)
            {
                if (image.MarkupObjects?.VerticalLinesCoordinates != null && image.MarkupObjects?.HorizontalLinesCoordinates != null)
                    counter += (image.MarkupObjects.VerticalLinesCoordinates.Count + 1) * (image.MarkupObjects.HorizontalLinesCoordinates.Count + 1);
            }

            return counter;
        }

        /// <summary>
        /// Wires up events
        /// </summary>
        public void WireUpEvents()
        {
            foreach (GalleryImage galleryImage in Images)
                galleryImage.RecognizableElementOcrCompleted += Image_RecognizableElementOcrCompleted;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds file to gallery
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="maxNumberOfVerticalLines">The maximum number of vertical lines.</param>
        /// <returns></returns>
        private ActionResult AddFile(string filePath, int maxNumberOfVerticalLines = 1)
        {
            if (Images == null)
                Images = new List<GalleryImage>();

            try
            {
                IFileTypeEngine engine = FileTypesEngineFactory.GetFileTypesEngine(filePath);
                int totalPages = engine.GetTotalPages(filePath);
                int orderNumber = Images.Any() ? Images.Max(x => x.OrderNumber) + 1 : 1;

                for (int i = 1; i <= totalPages; i++)
                {
                    Guid imageGuid = Guid.NewGuid();
                    string imageFileName = $"{imageGuid}.jpg";
                    string imagePath = Path.Combine(GalleryFolderPath, imageFileName);
                    string thumbnailFileName = $"{imageGuid}_thumb.jpg";
                    string thumbnailPath = Path.Combine(GalleryFolderPath, thumbnailFileName);

                    engine.SavePageToJpeg(filePath, imagePath, i);
                    new ImagesConverter(imagePath).CreateThumbnail(thumbnailPath, CommonSettings.ThumbnailWidth, CommonSettings.ThumbnailHeight);

                    GalleryImage galleryImage = new GalleryImage
                    {
                        Uid = Guid.NewGuid(),
                        OrderNumber = orderNumber,
                        DisplayedName = orderNumber.ToString(ImageNameMask),
                        ImageFolderPath = GalleryFolderPath,
                        ImageFileName = imageFileName,
                        ThumbnailFileName = thumbnailFileName,
                        RecognitionCompleted = false,
                        MarkupObjects = new MarkupObjects
                        {
                            MaxNumberOfVerticalLines = maxNumberOfVerticalLines
                        }                        
                    };
                    galleryImage.RecognizableElementOcrCompleted += Image_RecognizableElementOcrCompleted;

                    Images.Add(galleryImage);
                    orderNumber++;
                }

                return new ActionResult();
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e, $"Unable to add {filePath} to gallery");
                return new ActionResult(ActionResultType.Error, $"Error while trying to add {filePath} to gallery");
            }
        }

        #endregion

        #region Events handlers

        private void Image_RecognizableElementOcrCompleted(object sender, EventArgs eventArgs)
        {
            RecognizableElementOcrCompleted?.Invoke(this, null);
        }

        #endregion
    }
}