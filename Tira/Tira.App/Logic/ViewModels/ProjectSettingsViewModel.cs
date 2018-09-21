using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ak.Framework.Wpf.Commands;
using Ak.Framework.Wpf.Commands.Interfaces;
using Ak.Framework.Wpf.Dialogs;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Windows;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project settings
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class ProjectSettingsViewModel : TiraViewModelBase
    {
        #region Variables

        /// <summary>
        /// Data columns
        /// </summary>
        private BindingList<DataColumn> _dataColumns = new BindingList<DataColumn>();

        #endregion

        #region Properties

        /// <summary>
        /// Data columns
        /// </summary>
        public BindingList<DataColumn> DataColumns
        {
            get => _dataColumns;
            set
            {
                _dataColumns = value;
                OnPropertyChanged(() => DataColumns);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command for adding data column
        /// </summary>
        public INotifyCommand AddDataColumnCommand { get; }

        /// <summary>
        /// Command for editing data column
        /// </summary>
        public INotifyCommand EditDataColumnCommand { get; }

        /// <summary>
        /// Command for deleting data column
        /// </summary>
        public INotifyCommand DeleteDataColumnCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSettingsViewModel"/> class.
        /// </summary>
        /// <param name="dataColumns">Data columns</param>
        public ProjectSettingsViewModel(List<DataColumn> dataColumns)
        {
            _dataColumns = new BindingList<DataColumn>();
            if (dataColumns.Count > 0)
                foreach (DataColumn dataColumn in dataColumns)
                    _dataColumns.Add(dataColumn);

            OnPropertyChanged(() => DataColumns);

            AddDataColumnCommand = new NotifyCommand(_ => AddDataColumn());
            EditDataColumnCommand = new NotifyCommand(o => EditDataColumn((DataColumn)o));
            DeleteDataColumnCommand = new NotifyCommand(o => DeleteDataColumn((DataColumn)o));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds data column
        /// </summary>
        private void AddDataColumn()
        {
            DataColumnViewModel vm = new DataColumnViewModel();
            bool? result = ShowDialogAgent.Instance.ShowDialog<DataColumnWindow>(vm);
            if (result.HasValue && result.Value)
            {
                _dataColumns.Add(vm.DataColumn);
                OnPropertyChanged(() => DataColumns);
            }
        }

        /// <summary>
        /// Edit data column
        /// </summary>
        /// <param name="dataColumn">Data column</param>
        private void EditDataColumn(DataColumn dataColumn)
        {
            DataColumnViewModel vm = new DataColumnViewModel(dataColumn);
            bool? result = ShowDialogAgent.Instance.ShowDialog<DataColumnWindow>(vm);
            if (result.HasValue && result.Value)
            {
                DataColumn dc = DataColumns.FirstOrDefault(o => o == dataColumn);
                if (dc != null)
                {
                    dc = dataColumn;
                    OnPropertyChanged(() => DataColumns);
                }
            }
        }

        /// <summary>
        /// Deletes data column
        /// </summary>
        /// <param name="dataColumn">Data column</param>
        private void DeleteDataColumn(DataColumn dataColumn)
        {
            if (dataColumn != null)
            {
                DataColumns.Remove(dataColumn);
                OnPropertyChanged(() => DataColumns);
            }
        }

        #endregion
    }
}
