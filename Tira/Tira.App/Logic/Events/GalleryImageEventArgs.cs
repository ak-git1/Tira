using System;
using Tira.Logic.Models;

namespace Tira.App.Logic.Events
{
    /// <summary>
    /// Event arguments with gallery image
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class GalleryImageEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gallery image
        /// </summary>
        public GalleryImage Image { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryImageEventArgs"/> class.
        /// </summary>
        /// <param name="image">Image.</param>
        public GalleryImageEventArgs(GalleryImage image)
        {
            Image = image;
        }

        #endregion
    }
}
