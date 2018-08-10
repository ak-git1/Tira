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
using Tira.Logic.Models.Drawing;
using Tira.Logic.Settings;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Images gallery
    /// </summary>
    [Serializable]
    internal class Gallery
    {
        #region Variables and constants

        /// <summary>
        /// The image name mask
        /// </summary>
        private const string ImageNameMask = "0000000";

        #endregion

        #region Properties

        /// <summary>
        /// Gallery folder path
        /// </summary>
        [XmlElement]
        public string GalleryFolderPath { get; set; }

        /// <summary>
        /// Gallery folder path
        /// </summary>
        //[XmlIgnore]
        //public string GalleryAbsoluteFolderPath => 

        /// <summary>
        /// Gallery
        /// </summary>
        [XmlElement]
        public List<GalleryImage> Images { get; set; }

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
        /// Updates gallery pathes.
        /// </summary>
        /// <param name="galleryFolderPath">The gallery path.</param>
        public void UpdateGalleryPathes(string galleryFolderPath)
        {            
            GalleryFolderPath = galleryFolderPath;
            foreach (GalleryImage image in Images)
                image.ImageFolderPath = galleryFolderPath;
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
                        DrawingObjects = new DrawingObjects
                        {
                            MaxNumberOfVerticalLines = maxNumberOfVerticalLines
                        }
                    };

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
    }
}
