using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public static class ExcelHelper
    {
        public static void ExportToExcel<T>(string sheetName, List<T> data, List<ColumnConfig<T>> columns, Stream stream)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Header
            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = col.Header;
                cell.Style.Font.FontSize = 12;
                cell.Style.Font.Bold = true;
                //cell.Style.Alignment.WrapText = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Column(i + 1).Width = col.Width;
                worksheet.Column(i + 1).Style.Alignment.WrapText = true;

                if (!col.IsVisible)
                {
                    worksheet.Column(i + 1).Hide();
                }

                // Add column borders
                worksheet.Range(cell, cell)
                    .Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    .Border.SetOutsideBorderColor(XLColor.Black);
            }

            int lastDataRowIndex = data.Count - 1;

            // Data
            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                bool isTotalRow = row == lastDataRowIndex;

                for (int col = 0; col < columns.Count; col++)
                {
                    var cfg = columns[col];
                    var cell = worksheet.Cell(row + 2, col + 1);
                    object value = col == 0 ? (row + 1) : cfg.Selector(item);// show Sr.No if Col == 0

                    if (cfg.StyleAction != null)
                    {
                        cfg.StyleAction(cell, value, item);
                    }
                    else
                    {
                        if (value is DateTime dt)
                            cell.Value = dt.ToString("dd/MM/yyyy");
                        else if (value is decimal d)
                            cell.Value = d;
                        else if (value is int i)
                            cell.Value = i;
                        else
                            cell.Value = value?.ToString() ?? string.Empty;
                    }

                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                    cell.Style.Alignment.WrapText = true;


                    // If this is the total row, apply total row formatting
                    if (isTotalRow)
                    {
                        if (col == 0)
                        {
                            // do not show serial number if col == 0
                            cell.Value = "";
                        }

                        cell.Style.Font.FontSize = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    }


                    // Add column borders
                    worksheet.Range(cell, cell)
                        .Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        .Border.SetOutsideBorderColor(XLColor.Black);
                }
            }

            workbook.SaveAs(stream);
        }

    }
}
