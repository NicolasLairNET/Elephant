using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Windows.Forms;

namespace Elephant.Model
{
    public static class DataFile
    {
        private static string DataFilePath => $"{Directory.GetCurrentDirectory()}\\DATA.json";
        public static ObservableCollection<TDCTag> Tags { get; set; }

        public static ObservableCollection<TDCTag> Read()
        {
            if (!File.Exists(DataFilePath))
            {
                File.Create(DataFilePath).Close();
            }

            string data = File.ReadAllText(DataFilePath);
            if (data != "")
            {
                Tags = JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(data);
            }

            return Tags;
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


        public static ObservableCollection<TDCTag> Update()
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

            return Read();
        }

        public static ObservableCollection<TDCTag> Search(string tagName)
        {
            ObservableCollection<TDCTag> data = DataFile.Read();

            if (tagName != "")
            {
                Tags = new ObservableCollection<TDCTag>(
                from TDCTag in data
                where TDCTag.Name == tagName
                select TDCTag);
            }

            return Tags;
        }
    }
}
