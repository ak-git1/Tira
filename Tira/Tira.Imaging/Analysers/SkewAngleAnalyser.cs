using System;
using System.Drawing;
using GdPicture14;

namespace Tira.Imaging.Analysers
{
    /// <summary>
    /// Analyser for defining skew angle
    /// </summary>
    public static class SkewAngleAnalyser
    {
        #region Constants

        /// <summary>
        /// Maximum angle of deskew research
        /// </summary>
        private const float MaxAngleOfDeskewResearch = 60;

        /// <summary>
        /// Step of angle research
        /// </summary>
        private const float AngleResearchStep = 0.1f;

        #endregion

        #region Public methods

        /// <summary>
        /// Get skew angle
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="exludeBorders">Excluding borders from reaserch</param>
        /// <param name="percentageOfExcludableBorders">Percent of excluded borders</param>
        /// <returns></returns>
        public static float GetSkewAngle(Bitmap bitmap, bool exludeBorders = false, int percentageOfExcludableBorders = 0)
        {
            GdPictureImaging image = new GdPictureImaging();
            float angle = 0;

            try
            {
                int imageId = image.CreateGdPictureImageFromBitmap(bitmap);
                if (imageId > 0)
                {
                    if (exludeBorders)
                    {
                        int width = image.GetWidth(imageId);
                        int height = image.GetHeight(imageId);
                        int left = width / 100 * percentageOfExcludableBorders;
                        int top = width / 100 * percentageOfExcludableBorders;
                        image.Crop(imageId, left, top, width - 2 * left, height - 2 * top);
                    }
                    angle = image.GetSkewAngle(imageId, MaxAngleOfDeskewResearch, AngleResearchStep, true);
                    image.ReleaseGdPictureImage(imageId);
                }
            }
            catch (Exception e)
            {
            }

            return angle;
        }

        #endregion
    }
}
