using Ak.Framework.Wpf.ViewModels;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for data column
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class DataColumnViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Data column
        /// </summary>
        private DataColumn _dataColumn;

        #endregion

        #region Properties

        /// <summary>
        /// Data column
        /// </summary>
        public DataColumn DataColumn
        {
            get => _dataColumn;
            set
            {
                _dataColumn = value;
                OnPropertyChanged(() => DataColumn);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumnViewModel"/> class.
        /// </summary>
        public DataColumnViewModel()
        {
            DataColumn = new DataColumn();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumnViewModel" /> class.
        /// </summary>
        /// <param name="dataColumn">Data column</param>
        public DataColumnViewModel(DataColumn dataColumn)
        {
            DataColumn = dataColumn;
        }

        #endregion
    }
}
