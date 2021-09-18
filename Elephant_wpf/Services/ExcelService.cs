using System;
using System.IO;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace Elephant.Services
{
    class ExcelService
    {
        public void Export(DataGrid data)
        {
            string[] dataArray = ConvertDatagridToStringArray(data);
            Excel.Application excel = new();

            if (excel == null)
            {
                System.Windows.MessageBox.Show("Impossible d'exporter, Excel n'est pas installé");
                return;
            }

            Excel.Workbook excelWorkbook;
            Excel.Worksheet excelWorksheet;

            excelWorkbook = excel.Workbooks.Add(Missing.Value);
            excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(1);

            int line = 0;
            int colomn = 0;
            int colomnNumber = data.Columns.Count;

            for (int i = 0; i < dataArray.Length; i++)
            {
                if (i % colomnNumber == 0)
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

            string path = Path.Combine(SelectExportFolder(), $"export{DateTime.Now:ddMMyyyyHmmss}.xlsx");

            excelWorkbook.SaveAs(path);
            excelWorkbook.Close(true);
            excel.Quit();

            System.Windows.MessageBox.Show($"Export terminé dans {path}");
        }

        /// <summary>
        /// Convert data selected in datagrid to array of string.
        /// Convert all if nothing has been selected
        /// </summary>
        /// <param name="data"> Datagrid to convert </param>
        /// <returns>String array with the content of the selection</returns>
        private string[] ConvertDatagridToStringArray(DataGrid data)
        {
            // If nothing has been selected, sectioning all the rows
            if (data.SelectedItems.Count == 0)
            {
                data.SelectAllCells();
            }

            data.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, data);
            data.UnselectAllCells();

            string dataGridContent = (string)System.Windows.Clipboard.GetData(System.Windows.DataFormats.CommaSeparatedValue);

            return dataGridContent.Replace(Environment.NewLine, ",").Split(",");
        }

        private string SelectExportFolder()
        {
            string exportFolder = Directory.GetCurrentDirectory();
            FolderBrowserDialog folderBrowserDialog = new();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                exportFolder = folderBrowserDialog.SelectedPath;
            }

            return exportFolder;
        }
    }
}
