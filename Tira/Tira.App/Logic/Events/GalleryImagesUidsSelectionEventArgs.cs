using System;
using System.Collections.Generic;
using Ak.Framework.Core.Extensions;
using Tira.Logic.Models;

namespace Tira.App.Logic.Events
{
    /// <summary>
    /// Event arguments with gallery images ids selection
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class GalleryImagesUidsSelectionEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Identificators of selected gallery images
        /// </summary>
        public List<Guid> Uids { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryImageEventArgs"/> class.
        /// </summary>
        /// <param name="images">Images</param>
        public GalleryImagesUidsSelectionEventArgs(List<GalleryImage> images)
        {
            Uids = images.SelectEx(x => x.Uid).ToListEx();
        }

        #endregion
    }
}
