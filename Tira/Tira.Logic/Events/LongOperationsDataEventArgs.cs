using Tira.Logic.Models;

namespace Tira.Logic.Events
{
    /// <summary>
    /// Event arguments with data container for long running operations
    /// </summary>
    public class LongOperationsDataEventArgs
    {
        #region Properties

        /// <summary>
        /// Data
        /// </summary>
        public LongOperationsData LongOperationsData { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperationsDataEventArgs" /> class.
        /// </summary>
        /// <param name="data">Data</param>
        public LongOperationsDataEventArgs(LongOperationsData data)
        {
            LongOperationsData = data;
        }

        #endregion
    }
}
