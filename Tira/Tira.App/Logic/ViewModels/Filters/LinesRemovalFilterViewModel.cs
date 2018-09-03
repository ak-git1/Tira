using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Helpers;
using Ak.Framework.Wpf.Messaging;
using Tira.App.Logic.Enums;
using Tira.Imaging.Enums;
using Tira.Logic.Enums;

namespace Tira.App.Logic.ViewModels.Filters
{
    /// <summary>
    /// ViewModel for lines removal filter
    /// </summary>
    public class LinesRemovalFilterViewModel
    {
        #region Variables

        /// <summary>
        /// Orientation of lines to be removed
        /// </summary>
        private LineRemoveOrientation _lineRemoveOrientation = LineRemoveOrientation.None;

        /// <summary>
        /// Available line orientations
        /// </summary>
        private static List<LineRemoveOrientation> _availableLineRemoveOrientations = null;

        #endregion

        #region Properties

        /// <summary>
        /// Orientation of lines to be removed
        /// </summary>
        public LineRemoveOrientation LineRemoveOrientation
        {
            get => _lineRemoveOrientation;
            set
            {
                _lineRemoveOrientation = value;
                Messenger.Instance.Send(MessageType.SetFilterToSelectedImage, FilterType.LinesRemoval, _lineRemoveOrientation);
            }
        }

        /// <summary>
        /// List of available line orientations
        /// </summary>
        public static List<KeyValuePair<LineRemoveOrientation, string>> AvailableLineRemoveOrientationsList => AvailableLineRemoveOrientations.Select(l => new KeyValuePair<LineRemoveOrientation, string>(l, GetLineRemoveOrientationName(l))).ToList();

        /// <summary>
        /// Available line orientations
        /// </summary>
        private static List<LineRemoveOrientation> AvailableLineRemoveOrientations
        {
            get
            {
                if (_availableLineRemoveOrientations == null)
                    _availableLineRemoveOrientations = EnumNamesHelper.GetValues<LineRemoveOrientation>().ToList();
                return _availableLineRemoveOrientations;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets line orientation name
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        private static string GetLineRemoveOrientationName(LineRemoveOrientation item)
        {
            switch (item)
            {
                case LineRemoveOrientation.None:
                    return Properties.Resources.LineRemoveOrientation_None;

                case LineRemoveOrientation.Horizontal:
                    return Properties.Resources.LineRemoveOrientation_Horizontal;

                case LineRemoveOrientation.Vertical:
                    return Properties.Resources.LineRemoveOrientation_Vertical;

                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        #endregion
    }
}
