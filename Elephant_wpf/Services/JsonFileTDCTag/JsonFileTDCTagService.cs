using Elephant.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Elephant.Services
{
    class JsonFileTDCTagService
    {
        private static string JsonFileName => Path.Combine(Directory.GetCurrentDirectory(), "DATA.json");

        public JsonFileTDCTagService()
        {
            if (!File.Exists(JsonFileName))
            {
                StreamWriter sw = new(JsonFileName);
                sw.WriteLine("[]");
                sw.Close();
            }
        }

        public ObservableCollection<TDCTag> GetTDCTags()
        {
            try
            {
                using StreamReader jsonFileReader = File.OpenText(JsonFileName);
                return JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(
                    jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Le fichier de données à été supprimé, relancez l'application.");
                return null;
            }
        }

        public ObservableCollection<TDCTag> Import()
        {
            string[] filePathList = GetPathList();

            if (CreateJsonFile(filePathList))
            {
                MessageBox.Show("Import terminé");
            }
            else
            {
                MessageBox.Show("Aucun fichier importé");
            }

            return GetTDCTags();
        }

        public ObservableCollection<TDCTag> Search(string value)
        {
            ObservableCollection<TDCTag> data = GetTDCTags();
            Regex regex = new(StringToRegex(value));

            return value != ""
                ? new ObservableCollection<TDCTag>(
                from TDCTag in data
                let matchName = regex.Matches(TDCTag.Name)
                let matchValue = regex.Matches(TDCTag.Value)
                where matchName.Count > 0 || matchValue.Count > 0
                select TDCTag)
                : data;
        }

        private static string StringToRegex(string value)
        {
            return '^' + value.Replace('?', '.').Replace("*", ".*") + "$";
        }

        private bool CreateJsonFile(string[] filePathList)
        {
            if (!filePathList.Any())
            {
                return false;
            }

            List<TDCTag> tagList = new();

            foreach (string filePath in filePathList)
            {
                ITDCFile tdcFile = new TDCFileFactory(filePath).Create();
                if (tdcFile != null)
                {
                    tagList.AddRange(tdcFile.GetTagsList());
                }
                else
                {
                    MessageBox.Show($"Le fichier : {Path.GetFileName(filePath)} n'est pas pris en charge par Elephant");
                }
            }

            if (!tagList.Any())
            {
                return false;
            }

            // delete similar tags
            tagList = tagList.Distinct(new TDCTagComparer()).ToList();

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
