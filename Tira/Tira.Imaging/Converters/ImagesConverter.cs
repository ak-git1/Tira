using System;
using System.Drawing;
using GdPicture14;

namespace Tira.Imaging.Converters
{
    /// <summary>
    /// Images converter.
    /// </summary>
    public class ImagesConverter
    {
        #region Properties

        /// <summary>
        /// Image path
        /// </summary>
        public string ImagePath { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConverter"/> class.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        public ImagesConverter(string imagePath)
        {
            ImagePath = imagePath;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts to jpeg.
        /// </summary>
        /// <param name="outputPath">The output path.</param>
        public void ConvertToJpeg(string outputPath)
        {
            GdPictureImaging image = new GdPictureImaging();
            int imageId = image.CreateGdPictureImageFromFile(ImagePath);
            image.SaveAsJPEG(imageId, outputPath);
            image.ReleaseGdPictureImage(imageId);
            GC.Collect();
        }

        /// <summary>
        /// Creates the thumbnail.
        /// </summary>
        /// <param name="outputPath">The output path.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void CreateThumbnail(string outputPath, int width, int height)
        {
            GdPictureImaging image = new GdPictureImaging();
            int imageId = image.CreateGdPictureImageFromFile(ImagePath);
            int thumbnailId = image.CreateThumbnailHQ(imageId, width, height, Color.Black);
            image.SaveAsJPEG(thumbnailId, outputPath);
            image.ReleaseGdPictureImage(imageId);
            GC.Collect();
        }

        #endregion
    }
}
