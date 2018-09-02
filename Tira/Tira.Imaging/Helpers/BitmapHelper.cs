using System;
using System.Drawing;
using Ak.Framework.Imaging.Extensions;
using GdPicture14;
using Tira.Imaging.Analysers;
using Tira.Imaging.Classes;

namespace Tira.Imaging.Helpers
{
    /// <summary>
    /// Class for filtering and converting bitmaps
    /// </summary>
    public static class BitmapHelper
    {
        #region Public methods

        /// <summary>
        /// Set brightness for specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="brightness">Brightness (values from -100 to 100)</param>
        /// <returns></returns>
        public static Bitmap SetBrightness(Bitmap bitmap, int brightness)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.SetBrightness(imageId, brightness);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Set contrast for specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="contrast">Contrast (values from -100 to 100)</param>
        /// <returns></returns>
        public static Bitmap SetContrast(Bitmap bitmap, int contrast)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.SetContrast(imageId, contrast);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Set gamma for specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="gamma">Gamma (values from -100 to 100)</param>
        /// <returns></returns>
        public static Bitmap SetGammaCorrection(Bitmap bitmap, int gamma)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.SetGammaCorrection(imageId, gamma);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Crops the specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="rectangle">Rectangle</param>
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
        /// Auto crop borders
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static Bitmap AutoCropBorders(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

            try
            {
                if (imageId > 0)
                {
                    image.CropBorders(imageId, ImagingContext.ContextDocument);
                    resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                }
            }
            finally
            {
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Binarizes the specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static Bitmap Binarize(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.FxBlackNWhite(imageId, BitonalReduction.Stucki);
                image.ConvertTo1BppFast(imageId);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Binarizes the specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="threshold">Threshold</param>
        /// <returns></returns>
        public static Bitmap Binarize(Bitmap bitmap, byte threshold)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
            if (imageId > 0)
            {
                image.ConvertTo1Bpp(imageId, threshold);
                resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Dilates the specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
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
        /// Erodes the specified bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
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

        /// <summary>
        /// Removes the holes from holemaker
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="holesPosition">Holes position</param>
        /// <returns></returns>
        public static Bitmap RemoveHoles(Bitmap bitmap, HolesPosition holesPosition)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

            try
            {
                if (imageId > 0)
                {
                    if (holesPosition.LeftSide)
                        image.RemoveHolePunch(imageId, HolePunchMargins.MarginLeft);
                    if (holesPosition.TopSide)
                        image.RemoveHolePunch(imageId, HolePunchMargins.MarginTop);
                    if (holesPosition.RightSide)
                        image.RemoveHolePunch(imageId, HolePunchMargins.MarginRight);
                    if (holesPosition.BottomSide)
                        image.RemoveHolePunch(imageId, HolePunchMargins.MarginBottom);

                    resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                }
            }
            catch (Exception e)
            {                
            }
            finally
            {
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Removes blobs from image
        /// </summary>
        /// <param name="bitmap">Bitmap.</param>
        /// <param name="blobsDimensions">Blobs dimensions</param>
        /// <returns></returns>
        public static Bitmap RemoveBlobs(Bitmap bitmap, BlobsDimensions blobsDimensions)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

            try
            {
                if (imageId > 0)
                {
                    image.RemoveBlob(imageId, blobsDimensions.MinBlobWidth, blobsDimensions.MinBlobHeight, blobsDimensions.MaxBlobWidth, blobsDimensions.MaxBlobHeight, BlobRemoveMode.FavorQuality);
                    resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                }
            }
            finally
            {
                image.ReleaseGdPictureImage(imageId);

            }

            return resultImage;
        }

        /// <summary>
        /// Removes lines
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="orientation">Lines orientation</param>
        /// <returns></returns>
        public static Bitmap RemoveLines(Bitmap bitmap, Enums.LineRemoveOrientation orientation)
        {
            Bitmap resultImage = bitmap;

            LineRemoveOrientation lineRemoveOrientation;
            switch (orientation)
            {
                case Enums.LineRemoveOrientation.Horizontal:
                    lineRemoveOrientation = LineRemoveOrientation.Horizontal;
                    break;

                case Enums.LineRemoveOrientation.Vertical:
                    lineRemoveOrientation = LineRemoveOrientation.Vertical;
                    break;

                default:
                    return resultImage;
            }

            if (orientation != Enums.LineRemoveOrientation.None)
            {
                GdPictureImaging image = new GdPictureImaging();

                int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

                try
                {
                    if (imageId > 0)
                    {
                        image.RemoveLines(imageId, lineRemoveOrientation);
                        resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                    }
                }
                finally
                {
                    image.ReleaseGdPictureImage(imageId);
                }
            }

            return resultImage;
        }

        /// <summary>
        /// Removes staple marks
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static Bitmap RemoveStapleMarks(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

            try
            {
                if (imageId > 0)
                {
                    image.RemoveStapleMark(imageId);
                    resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                }
            }
            finally
            {
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Removes noise 
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static Bitmap RemoveNoise(Bitmap bitmap)
        {
            Bitmap resultImage = bitmap;
            GdPictureImaging image = new GdPictureImaging();

            int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

            try
            {
                if (imageId > 0)
                {
                    image.FxDespeckleMore(imageId);
                    resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                }
            }
            finally
            {
                image.ReleaseGdPictureImage(imageId);
            }

            return resultImage;
        }

        /// <summary>
        /// Rotates image
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="angle">Angle of rotation</param>
        /// <param name="fillColor">Fill color</param>
        /// <returns></returns>
        public static Bitmap Rotate(Bitmap bitmap, float angle, Color fillColor)
        {
            Bitmap resultImage = bitmap;
            if (angle != 0)
            {
                GdPictureImaging image = new GdPictureImaging();

                int imageId = image.CreateGdPictureImageFromBitmap(bitmap);

                try
                {
                    if (imageId > 0)
                    {
                        image.RotateAngleBackColor(imageId, angle, fillColor);
                        resultImage = image.GetBitmapFromGdPictureImage(imageId).CloneSmart(image.GetPixelFormat(imageId));
                    }
                }
                finally
                {
                    image.ReleaseGdPictureImage(imageId);
                }
            }

            return resultImage;
        }

        /// <summary>
        /// Auto deskew
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static Bitmap AutoDeskew(Bitmap bitmap)
        {
            return AutoDeskew(bitmap, new AutoDeskewParameters());
        }

        /// <summary>
        /// Auto deskew
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="autoDeskewParameters">Parameters of auto deskew</param>
        /// <returns></returns>
        public static Bitmap AutoDeskew(Bitmap bitmap, AutoDeskewParameters autoDeskewParameters)
        {
            float deskewAngle = SkewAngleAnalyser.GetSkewAngle(bitmap, autoDeskewParameters.RemoveBordersRequired, autoDeskewParameters.WidthOfBorders);

            return deskewAngle != 0 ?
                Rotate(bitmap, -deskewAngle, autoDeskewParameters.FillColor)
                : bitmap;
        }

        #endregion  
    }
}