using System;
using System.Data;
using System.IO;
using CsvHelper;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Csv export files generation engine
    /// </summary>
    /// <seealso cref="Tira.Logic.Engines.IExportFilesEngine" />
    internal class CsvExportFilesEngine : IExportFilesEngine
    {
        #region Public methods

        /// <summary>
        /// Generates the file.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="path">The path.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GenerateFile(DataTable dt, string path)
        {
            using (var textWriter = File.CreateText(path))
            using (var csv = new CsvWriter(textWriter))
            {
                foreach (DataColumn column in dt.Columns)
                    csv.WriteField(column.ColumnName);

                csv.NextRecord();

                foreach (DataRow row in dt.Rows)
                {
                    for (var i = 0; i < dt.Columns.Count; i++)
                        csv.WriteField(row[i]);
                    csv.NextRecord();
                }
            }
        }

        #endregion
    }
}
