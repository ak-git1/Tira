using Ak.Framework.Wpf.Messaging;
using Tira.App.Logic.Enums;
using Tira.Imaging.Classes;
using Tira.Logic.Enums;

namespace Tira.App.Logic.ViewModels.Filters
{
    /// <summary>
    /// ViewModel for blobs removal filter
    /// </summary>
    public class BlobsRemovalFilterViewModel
    {
        #region Variables        

        /// <summary>
        /// Blobs dimensions
        /// </summary>
        private BlobsDimensions _blobsDimensions;

        #endregion

        #region Properties

        /// <summary>
        /// Minimum width blob
        /// </summary>
        public int MinBlobWidth
        {
            get => _blobsDimensions.MinBlobWidth;
            set
            {
                _blobsDimensions.MinBlobWidth = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Maximum width blob
        /// </summary>
        public int MaxBlobWidth
        {
            get => _blobsDimensions.MaxBlobWidth;
            set
            {
                _blobsDimensions.MaxBlobWidth = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Minimum height blob
        /// </summary>
        public int MinBlobHeight
        {
            get => _blobsDimensions.MinBlobHeight;
            set
            {
                _blobsDimensions.MinBlobHeight = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Maximum height blob
        /// </summary>
        public int MaxBlobHeight
        {
            get => _blobsDimensions.MaxBlobHeight;
            set
            {
                _blobsDimensions.MaxBlobHeight = value;
                SendMessage();
            }
        }

        /// <summary>
        /// Blobs dimensions
        /// </summary>
        public BlobsDimensions BlobsDimensions
        {
            get => _blobsDimensions;
            set
            {
                _blobsDimensions = value;
                SendMessage();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobsRemovalFilterViewModel"/> class.
        /// </summary>
        public BlobsRemovalFilterViewModel()
        {
            BlobsDimensions = new BlobsDimensions
            {
                MinBlobWidth = 10,
                MinBlobHeight = 10,
                MaxBlobWidth = 30,
                MaxBlobHeight = 30
            };
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sends the message.
        /// </summary>
        private void SendMessage()
        {
            Messenger.Instance.Send(MessageType.SetFilterToSelectedImage, FilterType.BlobsRemoval, _blobsDimensions);
        }

        #endregion
    }
}
