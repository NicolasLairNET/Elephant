using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using Elephant.Model;

namespace Elephant.Services
{
    class ExportService
    {
        public void Export(List<TDCTag> tagList)
        {
            string path = SelectPathExport();
            if (path == null) return;

            string csvString = GenerateCsvString(tagList);
            File.AppendAllText(path, csvString, Encoding.UTF8);

            System.Windows.MessageBox.Show($"Export terminé dans {path}");
        }

        private string GenerateCsvString(List<TDCTag> tagList)
        {
            // create tags list with heading
            List<string> tags = new() {"Name", "Parameter", "Value", "Origin", Environment.NewLine};

            foreach (TDCTag tag in tagList)
            {
                tags.AddRange(tag.ToList());
            }

            return ConvertListToStringCsv(tags);
        }

        private string ConvertListToStringCsv(List<string> list)
        {
            return string.Join(",", list.ToArray()).Replace(Environment.NewLine + ",", Environment.NewLine);
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
