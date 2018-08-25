using System;
using Tira.Logic.Enums;

namespace Tira.Logic.Helpers
{
    /// <summary>
    /// Class for working with export data formats
    /// </summary>
    public static class ExportFormatHelper
    {
        /// <summary>
        /// Gets file extension
        /// </summary>
        /// <param name="exportFormat">Export format</param>
        /// <returns></returns>
        public static string GetFileExtension(ExportFormat exportFormat)
        {
            switch (exportFormat)
            {
                case ExportFormat.Json:
                    return "json";

                case ExportFormat.Xls:
                    return "xls";

                case ExportFormat.Xlsx:
                    return "xlsx";

                case ExportFormat.Csv:
                    return "csv";

                default:
                    throw new ArgumentOutOfRangeException(nameof(exportFormat), exportFormat, null);
            }
        }

        /// <summary>
        /// Gets file filter for save dialog
        /// </summary>
        /// <param name="exportFormat">Export format</param>
        /// <returns></returns>
        public static string GetFileFilterForSaveDialog(ExportFormat exportFormat)
        {
            switch (exportFormat)
            {
                case ExportFormat.Json:
                    return "*.json|*.json";

                case ExportFormat.Xls:
                    return "*.xls|*.xls";

                case ExportFormat.Xlsx:
                    return "*.xlsx|*.xlsx";

                case ExportFormat.Csv:
                    return "*.csv|*.csv";

                default:
                    throw new ArgumentOutOfRangeException(nameof(exportFormat), exportFormat, null);
            }
        }
    }
}
