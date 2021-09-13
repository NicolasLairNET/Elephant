using System;
using System.IO;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Input;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace Elephant.Services
{
    class ExcelService
    {
        private string ExportPath { get; }

        public ExcelService(string exportPath = "Downloads")
        {
            ExportPath = exportPath;
        }

        public void Export(DataGrid data)
        {
            string[] dataArray = DataGridToStringArray(data);

            Excel.Application excel = new();

            if (excel == null)
            {
                MessageBox.Show("Impossible d'exporter, Excel n'est pas installé");
                return;
            }

            Excel.Workbook excelWorkbook;
            Excel.Worksheet excelWorksheet;

            excelWorkbook = excel.Workbooks.Add(Missing.Value);
            excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(1);

            excelWorksheet.Cells[1, 1] = dataArray[0];
            excelWorksheet.Cells[1, 2] = dataArray[1];
            excelWorksheet.Cells[1, 3] = dataArray[2];
            excelWorksheet.Cells[1, 4] = dataArray[3];

            int line = 1;
            int colomn = 0;

            for (int i = 4; i < dataArray.Length; i++)
            {
                if (i % 4 == 0)
                {
                    line++;
                    colomn = 1;
                }
                else
                {
                    colomn++;
                }
                excelWorksheet.Cells[line, colomn] = dataArray[i];
            }

            string path = Path.Combine("\\", ExportPath, $"export{DateTime.Now:ddMMyyyyHmmss}.xlsx");
            excelWorkbook.SaveAs(path);
            excelWorkbook.Close(true);
            excel.Quit();

            MessageBox.Show($"Export terminé dans {path}");
        }

        private string[] DataGridToStringArray(DataGrid data)
        {
            data.SelectAllCells();
            data.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, data);
            data.UnselectAllCells();

            string dataGridContent = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

            return dataGridContent.Replace(Environment.NewLine, ",").Split(",");
        }
    }
}
