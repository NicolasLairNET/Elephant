using Elephant.ViewModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace Elephant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new TDCTagViewModel();
        }

        private void SearchTag_Click(object sender, RoutedEventArgs e)
        {
            TDCTagViewModel vm = (TDCTagViewModel)DataContext;
            vm.Search(txtName.Text);
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            TDCTagViewModel vm = (TDCTagViewModel)DataContext;
            vm.Update();
        }

        private void SearchTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TDCTagViewModel vm = (TDCTagViewModel)DataContext;
                vm.Search(txtName.Text);
            }
        }

        private void BtnExportData_Click(object sender, RoutedEventArgs e)
        {
            DGridResult.SelectAllCells();
            DGridResult.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DGridResult);
            DGridResult.UnselectAllCells();

            string result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            result = result.Replace(System.Environment.NewLine, ",");
            string[] test = result.Split(",");


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

            excelWorksheet.Cells[1, 1] = test[0];
            excelWorksheet.Cells[1, 2] = test[1];
            excelWorksheet.Cells[1, 3] = test[2];
            excelWorksheet.Cells[1, 4] = test[3];

            int nbLigne = 1;
            int nbColonne = 0;


            for (int i = 4; i < test.Length; i++)
            {
                if (i % 4 == 0)
                {
                    nbLigne++;
                    nbColonne = 1;
                }
                else
                {
                    nbColonne++;
                }
                excelWorksheet.Cells[nbLigne, nbColonne] = test[i];
            }

            excelWorkbook.SaveAs("export.xlsx");
            excelWorkbook.Close(true);
            excel.Quit();
        }
    }
}
