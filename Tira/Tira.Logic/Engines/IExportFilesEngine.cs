using System.Data;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Interface for export files engine
    /// </summary>
    internal interface IExportFilesEngine
    {
        #region Public methods

        void GenerateFile(DataTable dt, string path);

        #endregion
    }
}
