using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ElephantLibrary
{
    public class DataFile
    {
        private string DataFilePath => $"{Directory.GetCurrentDirectory()}\\DATA.json";

        public ObservableCollection<TDCTag> Tags { get; set; }

        public DataFile()
        {
            string data = File.ReadAllText(DataFilePath);
            Tags = JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(data);
        }


        private bool Create(string[] filePathList)
        {
            List<TDCTag> tagList = new();

            foreach (string filePath in filePathList)
            {
                string fileExtension = Path.GetExtension(filePath);
                string fileName = Path.GetFileName(filePath);

                ITDCFile tdcFile = fileExtension switch
                {
                    ".EB" => new EBFile(filePath, fileName),
                    ".XX" => new XXFile(filePath, fileName),
                    _ => null
                };

                tagList.AddRange(tdcFile.Read());
            }
            string tagListSerialized = JsonSerializer.Serialize(tagList);
            File.WriteAllText(DataFilePath, tagListSerialized);

            return true;
        }


        public List<TDCTag> Search(string tagName)
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

        public void UpdateData()
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
