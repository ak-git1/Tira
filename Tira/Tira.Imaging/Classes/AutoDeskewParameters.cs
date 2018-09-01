using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tira.Imaging.Enums;

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