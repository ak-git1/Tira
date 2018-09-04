using System.Data;
using System.IO;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Xls export files generation engine
    /// </summary>
    /// <seealso cref="Tira.Logic.Engines.IExportFilesEngine" />
    internal class XlsExportFilesEngine : IExportFilesEngine
    {
        #region Public methods

        /// <summary>
        /// Generates export file
        /// </summary>
        /// <param name="dt">Data table</param>
        /// <param name="path">Path</param>
        public void GenerateFile(DataTable dt, string path)
        {
            IWorkbook workbook = new HSSFWorkbook();

            ISheet worksheet = workbook.CreateSheet("data");

            GenerateStructure(worksheet, dt.Rows.Count + 1, dt.Columns.Count);
            ICellStyle headerCellStyle = CreateHeaderCellStyle(workbook);
            ICellStyle dataCellStyle = CreateDataCellStyle(workbook);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn dataColumn = dt.Columns[i];
                worksheet.SetColumnWidth(i, 10000);

                ICell headerCell = GetCell(worksheet, 0, i);
                headerCell.SetCellValue(dataColumn.ColumnName);
                headerCell.CellStyle = headerCellStyle;

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    ICell cell = GetCell(worksheet, j + 1, i);
                    cell.SetCellValue(dt.Rows[j][i].ToStr());
                    cell.CellStyle = dataCellStyle;
                }
            }

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                workbook.Write(stream);
            }

            GarbageCollector.Collect();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the cell
        /// </summary>
        /// <param name="worksheet">Worksheet.</param>
        /// <param name="rowNumber">Row number</param>
        /// <param name="columnNumber">Column number</param>
        /// <returns></returns>
        private ICell GetCell(ISheet worksheet, int rowNumber, int columnNumber)
        {
            CellReference cr = new CellReference(rowNumber, columnNumber);
            IRow row = worksheet.GetRow(cr.Row);
            return row.GetCell(cr.Col);
        }

        /// <summary>
        /// Generates worksheet structure
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <param name="rowNumber">Row number</param>
        /// <param name="columnNumber">Column number</param>
        private void GenerateStructure(ISheet worksheet, int rowNumber, int columnNumber)
        {
            for (int row = 0; row < rowNumber; row++)
            {
                IRow r = worksheet.CreateRow(row);
                for (int cell = 0; cell < columnNumber; cell++)
                    r.CreateCell(cell);
            }
        }

        /// <summary>
        /// Creates header cell style
        /// </summary>
        /// <param name="workbook">Workbook.</param>
        /// <returns></returns>
        private ICellStyle CreateHeaderCellStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderLeft = BorderStyle.Thin;
            style.LeftBorderColor = IndexedColors.Black.Index;
            style.BorderTop = BorderStyle.Thin;
            style.TopBorderColor = IndexedColors.Black.Index;
            style.BorderRight = BorderStyle.Thin;
            style.RightBorderColor = IndexedColors.Black.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BottomBorderColor = IndexedColors.Black.Index;

            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = IndexedColors.DarkGreen.Index;

            style.WrapText = true;

            IFont font = workbook.CreateFont();
            font.IsBold = true;
            font.Color = IndexedColors.White.Index;
            style.SetFont(font);

            return style;
        }

        /// <summary>
        /// Creates data cell style
        /// </summary>
        /// <param name="workbook">Workbook.</param>
        /// <returns></returns>
        private ICellStyle CreateDataCellStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.BorderLeft = BorderStyle.Thin;
            style.LeftBorderColor = IndexedColors.Black.Index;
            style.BorderTop = BorderStyle.Thin;
            style.TopBorderColor = IndexedColors.Black.Index;
            style.BorderRight = BorderStyle.Thin;
            style.RightBorderColor = IndexedColors.Black.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BottomBorderColor = IndexedColors.Black.Index;

            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = IndexedColors.White.Index;

            style.WrapText = true;

            return style;
        }

        #endregion
    }
}
