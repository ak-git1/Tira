using System.Data;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Interface for export files engine
    /// </summary>
    internal interface IExportFilesEngine
    {
        #region Public methods

        /// <summary>
        /// Generates export file
        /// </summary>
        /// <param name="dt">Data table</param>
        /// <param name="path">Path</param>
        void GenerateFile(DataTable dt, string path);

        #endregion
    }
}
