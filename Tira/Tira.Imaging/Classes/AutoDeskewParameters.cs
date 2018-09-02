using System.Drawing;

namespace Tira.Imaging.Classes
{
    /// <summary>
    /// Parameters for auto deskew operation
    /// </summary>
    public class AutoDeskewParameters
    {
        #region Properties

        /// <summary>
        /// Color of fill
        /// </summary>
        public Color FillColor { get; set; } = Color.Black;

        /// <summary>
        /// Require borders removal
        /// </summary>
        public bool RemoveBordersRequired { get; set; } = false;

        /// <summary>
        /// The ratio of the width of the edges to the corresponding image dimensions
        /// </summary>
        public int WidthOfBorders { get; set; } = 10;

        #endregion
    }
}