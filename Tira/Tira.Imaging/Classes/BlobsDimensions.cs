namespace Tira.Imaging.Classes
{
    /// <summary>
    /// Dimensions for blobs removal
    /// </summary>
    public class BlobsDimensions
    {
        #region Properties

        /// <summary>
        /// Minimum width blob
        /// </summary>
        public int MinBlobWidth { get; set; }

        /// <summary>
        /// Maximum width blob
        /// </summary>
        public int MaxBlobWidth { get; set; }

        /// <summary>
        /// Minimum height blob
        /// </summary>
        public int MinBlobHeight { get; set; }

        /// <summary>
        /// Maximum height blob
        /// </summary>
        public int MaxBlobHeight { get; set; }

        #endregion
    }
}
