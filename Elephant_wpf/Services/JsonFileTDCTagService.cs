using Elephant.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Elephant.Services
{
    class JsonFileTDCTagService
    {
        private string JsonFileName
        {
            get => Path.Combine(Directory.GetCurrentDirectory(), "DATA.json");
        }

        public JsonFileTDCTagService()
        {
            if (!File.Exists(JsonFileName))
            {
                StreamWriter sw = new(JsonFileName);
                sw.WriteLine("[]");
                sw.Close();
            }
        }

        public ObservableCollection<TDCTag> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(
                    jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public ObservableCollection<TDCTag> Update()
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

            return GetProducts();
        }

        public ObservableCollection<TDCTag> Search(string tagName)
        {
            ObservableCollection<TDCTag> data = GetProducts();

            if (tagName != "")
            {
                return new ObservableCollection<TDCTag>(
                from TDCTag in data
                where TDCTag.Name == tagName
                select TDCTag);
            }

            return data;
        }

        private bool Create(string[] filePathList)
        {
            if (!filePathList.Any())
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
                    tdcFile = new EBFile(filePath);
                }
                else if (fileExtension == ".XX")
                {
                    tdcFile = new XXFile(filePath);
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
            File.WriteAllText(JsonFileName, tagListSerialized);

            return true;
        }

        /// <summary>
        /// Open a fileDialog for import TDC Files
        /// </summary>
        /// <returns>List of TDC Files's path</returns>
        private string[] GetPathList()
        {
            string[] pathList = new string[0];
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = "c:\\",
                RestoreDirectory = true,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathList = openFileDialog.FileNames;
            }

            return pathList;
        }


    }
}
