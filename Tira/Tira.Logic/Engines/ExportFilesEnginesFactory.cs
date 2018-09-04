using System;
using Tira.Logic.Enums;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Factory for export files engines
    /// </summary>
    internal static class ExportFilesEnginesFactory
    {
        /// <summary>
        /// Creates export files engine
        /// </summary>
        /// <param name="exportFormat">Export format</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">exportFormat - null</exception>
        public static IExportFilesEngine GetExportFilesEngine(ExportFormat exportFormat)
        {
            switch (exportFormat)
            {
                case ExportFormat.Json:
                    return new JsonExportFilesEngine();

                case ExportFormat.Xls:
                    return new XlsExportFilesEngine();

                case ExportFormat.Xlsx:
                    return new XlsxExportFilesEngine();

                case ExportFormat.Csv:
                    return new CsvExportFilesEngine();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exportFormat), exportFormat, null);
            }
        }
    }
}
