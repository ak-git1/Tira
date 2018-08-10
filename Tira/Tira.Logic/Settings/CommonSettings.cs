using Ak.Framework.Core.Extensions;
using System.Configuration;

namespace Tira.Logic.Settings
{
    /// <summary>
    /// Common settings
    /// </summary>
    public static class CommonSettings
    {
        #region Variables

        /// <summary>
        /// Thumbnail width for image in gallery
        /// </summary>
        private static int? _thumbnailWidth;

        /// <summary>
        /// Thumbnail height for image in gallery
        /// </summary>
        private static int? _thumbnailHeight;

        #endregion

        #region Properties

        /// <summary>
        /// Thumbnail width for image in gallery
        /// </summary>
        public static int ThumbnailWidth
        {
            get
            {
                if (_thumbnailWidth == null)
                    _thumbnailWidth = ConfigurationManager.AppSettings["ThumbnailWidth"].ToInt32();
                return _thumbnailWidth.Value;
            }
        }

        /// <summary>
        /// Thumbnail height for image in gallery
        /// </summary>
        public static int ThumbnailHeight
        {
            get
            {
                if (_thumbnailHeight == null)
                    _thumbnailHeight = ConfigurationManager.AppSettings["ThumbnailHeight"].ToInt32();
                return _thumbnailHeight.Value;
            }
        }

        #endregion
    }
}
