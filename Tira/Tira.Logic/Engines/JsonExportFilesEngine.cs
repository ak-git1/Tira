using System;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Json export files generation engine
    /// </summary>
    /// <seealso cref="Tira.Logic.Engines.IExportFilesEngine" />
    internal class JsonExportFilesEngine : IExportFilesEngine
    {
        #region Public methods

        /// <summary>
        /// Generates export file
        /// </summary>
        /// <param name="dt">Data table</param>
        /// <param name="path">Path</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GenerateFile(DataTable dt, string path)
        {
            string json = JsonConvert.SerializeObject(dt);
            File.WriteAllText(path, json);
        }

        #endregion
    }
}