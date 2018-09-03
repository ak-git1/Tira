using System;
using System.Drawing;

namespace Tira.App.Logic.Events
{
    /// <summary>
    /// Event arguments with rectangle
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class RectangleAreaEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Rectangle
        /// </summary>
        public Rectangle Rectangle { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleAreaEventArgs" /> class.
        /// </summary>
        /// <param name="rectangle">Rectangle.</param>
        public RectangleAreaEventArgs(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        #endregion
    }
}
