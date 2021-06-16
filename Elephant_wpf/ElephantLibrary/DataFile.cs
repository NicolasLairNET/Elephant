using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ElephantLibrary
{
    public static class DataFile
    {
        private static string DataFilePath => $"{Directory.GetCurrentDirectory()}\\DATA.json";

        private static void Create(string[] fileList)
        {
            List<TDCTag> pointList = new();

            foreach (string fileName in fileList)
            {
                string fileExtension = Path.GetExtension(fileName);
                ITDCFile tdcFile = fileExtension switch
                {
                    ".EB" => new EBFile(fileName),
                    _ => null
                };

                List<TDCTag> EBList = tdcFile.Read();
                pointList.AddRange(EBList);
            }
            string pointListSerialized = JsonSerializer.Serialize(pointList);
            File.WriteAllText(DataFilePath, pointListSerialized);
        }


        public static List<TDCTag> Search(string tagName)
        {
            string data = File.ReadAllText(DataFilePath);
            List<TDCTag> pointList = JsonSerializer.Deserialize<List<TDCTag>>(data);

            List<TDCTag> result = pointList.FindAll(
                delegate (TDCTag p)
                {
                    return p.Name == tagName || p.Value == tagName;
                }
            );

            return result;
        }

        public static void UpdateData()
        {
            using OpenFileDialog openFileDialog = new();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "EB files (*.EB)|*.EB|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Create(openFileDialog.FileNames);
            }

            System.Windows.MessageBox.Show("Import terminé");
        }

    }
}
