using Ak.Framework.Core.Extensions;

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

        /// <summary>
        /// Maximum number of recent projects
        /// </summary>
        private static int? _maxNumberOfRecentProjects;

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
                    _thumbnailWidth = SettingsHelper.GetValue("ThumbnailWidth").ToInt32();
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
                    _thumbnailHeight = SettingsHelper.GetValue("ThumbnailHeight").ToInt32();
                return _thumbnailHeight.Value;
            }
        }

        /// <summary>
        /// Maximum number of recent projects
        /// </summary>
        public static int MaxNumberOfRecentProjects
        {
            get
            {
                if (_maxNumberOfRecentProjects == null)
                    _maxNumberOfRecentProjects = SettingsHelper.GetValue("MaxNumberOfRecentProjects").ToInt32();               
                return _maxNumberOfRecentProjects.Value;
            }
        }

        #endregion
    }
}