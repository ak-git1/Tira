using System.Drawing;
using GdPicture14;
using Tira.Imaging.Extensions;

namespace Tira.Imaging.Helpers
{
    /// <summary>
    /// Class for filtering and converting bitmaps
    /// </summary>
    public static class BitmapHelper
    {
        #region Public methods

        /// <summary>
        /// Crops the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns></returns>
        public static Bitmap Crop(Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.Crop(imageId, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;

        }

        /// <summary>
        /// Binarizes the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static Bitmap Binarize(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.FxBlackNWhite(imageId, BitonalReduction.Stucki);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Dilates the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static Bitmap Dilate(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.FxDilate(imageId);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Erodes the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static Bitmap Erode(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.FxErode(imageId);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        #endregion
    }
}
