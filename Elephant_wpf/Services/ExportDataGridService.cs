using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.Text;

namespace Elephant.Services
{
    class ExportDataGridService
    {
        public void Export(DataGrid datagrid)
        {
            string pathExport = SelectPathExport();
            if (pathExport != null)
            {
                string data = ConvertDatagridToString(datagrid);

                File.AppendAllText(pathExport, data, UnicodeEncoding.UTF8);

                System.Windows.MessageBox.Show($"Export terminé dans {pathExport}");
            }
        }

        /// <summary>
        /// Convert data selected in datagrid to array of string.
        /// Convert all if nothing has been selected
        /// </summary>
        /// <param name="data"> Datagrid to convert </param>
        /// <returns>String array with the content of the selection</returns>
        private string ConvertDatagridToString(DataGrid data)
        {
            // If nothing has been selected, sectioning all the rows
            if (data.SelectedItems.Count == 0)
            {
                data.SelectAllCells();
            }

            data.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, data);
            data.UnselectAllCells();

            return (string)System.Windows.Clipboard.GetData(System.Windows.DataFormats.CommaSeparatedValue);
        }

        private string SelectPathExport()
        {
            string defaultFileName = $"export{DateTime.Now:ddMMyyyyHmmss}.csv";
            string defaultPath = Path.Combine(Directory.GetCurrentDirectory());

            SaveFileDialog saveFileDialog = new();
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.InitialDirectory = defaultPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
