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
            if (filePathList == null)
            {
                return false;
            }

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

            if (!tagList.Any())
            {
                return false;
            }

            string tagListSerialized = JsonSerializer.Serialize(tagList);
            File.WriteAllText(DataFilePath, tagListSerialized);

            return true;
        }

        public static ObservableCollection<TDCTag> Update()
        {
            string[] filePathList = GetPathList();

            if (Create(filePathList))
            {
                System.Windows.MessageBox.Show("Import terminé");
            }
            else
            {
                System.Windows.MessageBox.Show("Aucun fichier importé");
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

        /// <summary>
        /// Open a fileDialog for import TDC Files
        /// </summary>
        /// <returns>List of TDC Files's path</returns>
        private static string[] GetPathList()
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = "c:\\",
                RestoreDirectory = true,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileNames;
            }

            return null;
        }
    }
}
