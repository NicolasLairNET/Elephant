using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Elephant.Model
{
    public class DataFile
    {
        private static string DataFilePath => $"{Directory.GetCurrentDirectory()}\\DATA.json";

        public ObservableCollection<TDCTag> Tags { get; set; }

        public DataFile()
        {
            string data = Read();
            if (data != "")
            {
                Tags = JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(data);
            }
        }

        private string Read()
        {
            if (!File.Exists(DataFilePath))
            {
                File.Create(DataFilePath).Close();
            }

            return File.ReadAllText(DataFilePath);
        }


        private static bool Create(string[] filePathList)
        {
            List<TDCTag> tagList = new();

            foreach (string filePath in filePathList)
            {
                string fileExtension = Path.GetExtension(filePath);
                string fileName = Path.GetFileName(filePath);
                ITDCFile tdcFile;
                if (fileExtension == ".EB")
                {
                    tdcFile = new EBFile(filePath, fileName);
                }
                else if (fileExtension == ".XX")
                {
                    tdcFile = new XXFile(filePath, fileName);
                }
                else
                {
                    continue;
                }

                tagList.AddRange(tdcFile.Read());
            }
            string tagListSerialized = JsonSerializer.Serialize(tagList);
            File.WriteAllText(DataFilePath, tagListSerialized);

            return true;
        }


        public static void UpdateData()
        {
            using OpenFileDialog openFileDialog = new();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "EB files (*.EB)|*.EB|XX files (*.XX)|*.XX|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (Create(openFileDialog.FileNames))
                {
                    System.Windows.MessageBox.Show("Import terminé");
                }
                else
                {
                    System.Windows.MessageBox.Show("Erreur: echec de l'import");
                }
            }
        }
    }
}
