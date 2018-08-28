using System;
using Tira.Logic.Models;
using Tira.Logic.Models.Markup;

namespace Tira.App.Logic.Events
{
    /// <summary>
    /// Event arguments with markup objects
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class MarkupObjectsEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Markup objects
        /// </summary>
        public MarkupObjects MarkupObjects { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupObjectsEventArgs"/> class.
        /// </summary>
        /// <param name="markupObjects">Markup objects</param>
        public MarkupObjectsEventArgs(MarkupObjects markupObjects)
        {
            MarkupObjects = markupObjects;
        }

        #endregion
    }
}
