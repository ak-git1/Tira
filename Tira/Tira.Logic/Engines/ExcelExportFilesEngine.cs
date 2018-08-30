using System.Data;
using System.Drawing;
using System.IO;
using Ak.Framework.Core.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Tira.Logic.Engines
{
    /// <summary>
    /// Excel export files generation engine
    /// </summary>
    /// <seealso cref="Tira.Logic.Engines.IExportFilesEngine" />
    internal class ExcelExportFilesEngine : IExportFilesEngine
    {
        #region Public methods

        /// <summary>
        /// Generates export file
        /// </summary>
        /// <param name="dt">Data table</param>
        /// <param name="path">Path</param>
        public void GenerateFile(DataTable dt, string path)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("data");

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn dataColumn = dt.Columns[i];
                    worksheet.Column(i + 1).Width = 55;
                    SetHeaderCell(worksheet, dataColumn.ColumnName, GetCellName(i + 1, 1));

                    for (int j = 0; j < dt.Rows.Count; j++)
                        SetDataCell(worksheet, dt.Rows[j][i].ToStr(), GetCellName(i + 1, j + 2));
                }

                package.SaveAs(new FileInfo(path));
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the name of the cell
        /// </summary>
        /// <param name="rowNumber">Row number</param>
        /// <param name="cellNumber">Cell number</param>
        /// <returns></returns>
        private string GetCellName(int rowNumber, int cellNumber)
        {
            return $"{rowNumber.ToLetter(true)}{cellNumber}";
        }

        /// <summary>
        /// Sets header line cell
        /// </summary>
        /// <param name="worksheet">Worksheet.</param>
        /// <param name="data">Data</param>
        /// <param name="cellName">Name of the cell</param>
        private void SetHeaderLineCell(ExcelWorksheet worksheet, string data, string cellName)
        {
            ExcelRange cells = worksheet.Cells[cellName];
            cells.Value = data;
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BDD7EE"));
            cells.Style.Font.Size = 20;
            cells.Style.Font.Bold = true;
        }

        /// <summary>
        /// Sets the header cell
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <param name="data">Data</param>
        /// <param name="cellName">Name of the cell</param>
        private void SetHeaderCell(ExcelWorksheet worksheet, string data, string cellName)
        {
            ExcelRange cells = worksheet.Cells[cellName];
            cells.Value = data;
            cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#5B9BD5"));
            cells.Style.Font.Color.SetColor(Color.White);
            cells.Style.Font.Bold = true;
            cells.Style.WrapText = true;
        }

        /// <summary>
        /// Sets the data cell
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <param name="data">Data</param>
        /// <param name="cellName">Name of the cell</param>
        /// <param name="isBold">Is bold</param>
        private void SetDataCell(ExcelWorksheet worksheet, string data, string cellName, bool isBold = false)
        {
            ExcelRange cells = worksheet.Cells[cellName];
            cells.Value = data;
            cells.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDEBF7"));
            cells.Style.WrapText = true;
            cells.Style.Font.Bold = isBold;
        }

        #endregion
    }
}
