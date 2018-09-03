using Ak.Framework.Wpf.Messaging;
using Tira.App.Logic.Enums;
using Tira.Logic.Enums;

namespace Tira.App.Logic.ViewModels.Filters
{
    /// <summary>
    /// ViewModel for filters with one int parameter
    /// </summary>
    public class IntValueFilterViewModel
    {
        #region Variables

        /// <summary>
        /// Int value
        /// </summary>
        private int _intValue;

        /// <summary>
        /// Filter type
        /// </summary>
        private FilterType _filterType;

        #endregion

        #region Properties

        /// <summary>
        /// Int value
        /// </summary>
        public int IntValue
        {
            get => _intValue;
            set
            {
                _intValue = value;
                Messenger.Instance.Send(MessageType.SetFilterToSelectedImage, _filterType, _intValue);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntValueFilterViewModel" /> class.
        /// </summary>
        /// <param name="filterType">Filter type</param>
        /// <param name="value">Value</param>
        public IntValueFilterViewModel(FilterType filterType, int value)
        {
            _filterType = filterType;
            _intValue = value;
        }

        #endregion
    }
}