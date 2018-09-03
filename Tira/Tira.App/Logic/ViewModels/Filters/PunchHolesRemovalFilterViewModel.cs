using Ak.Framework.Wpf.Messaging;
using Tira.App.Logic.Enums;
using Tira.Imaging.Classes;
using Tira.Logic.Enums;

namespace Tira.App.Logic.ViewModels.Filters
{
    /// <summary>
    /// ViewModel for punch holes removal filter
    /// </summary>
    public class PunchHolesRemovalFilterViewModel
    {
        #region Variables

        /// <summary>
        /// Punch holes positions
        /// </summary>
        private PunchHolesPositions _punchHolesPositions;

        #endregion

        #region Properties

        /// <summary>
        /// Left side punch holes position
        /// </summary>
        public bool LeftSide
        {
            get => _punchHolesPositions.LeftSide;
            set
            {
                _punchHolesPositions.LeftSide = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Top side punch holes position
        /// </summary>
        public bool TopSide
        {
            get => _punchHolesPositions.TopSide;
            set
            {
                _punchHolesPositions.TopSide = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Right side punch holes position
        /// </summary>
        public bool RightSide
        {
            get => _punchHolesPositions.RightSide;
            set
            {
                _punchHolesPositions.RightSide = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Bottom side punch holes position
        /// </summary>
        public bool BottomSide
        {
            get => _punchHolesPositions.BottomSide;
            set
            {
                _punchHolesPositions.BottomSide = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Punch holes positions
        /// </summary>
        public PunchHolesPositions PunchHolesPositions
        {
            get => _punchHolesPositions;
            set
            {
                _punchHolesPositions = value;
                SendMessage();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PunchHolesRemovalFilterViewModel"/> class.
        /// </summary>
        public PunchHolesRemovalFilterViewModel()
        {
            PunchHolesPositions = new PunchHolesPositions();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sends the message.
        /// </summary>
        private void SendMessage()
        {
            Messenger.Instance.Send(MessageType.SetFilterToSelectedImage, FilterType.PunchHolesRemoval, _punchHolesPositions);
        }

        #endregion
    }
}