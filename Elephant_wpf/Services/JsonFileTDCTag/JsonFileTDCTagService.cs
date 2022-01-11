using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.Helpers;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System.Windows.Forms;

namespace Elephant.Services
{
    public sealed class JsonFileTdcTagService : IJsonTdcTagService
    {
        public string SavedFile { get; set; }
        public List<TDCTag> TDCTags { get; set; }

        private static JsonFileTdcTagService? instance;
        private static readonly object instanceLock = new();

        private JsonFileTdcTagService()
        {
            SavedFile = "";
            TDCTags = new List<TDCTag>();
        }

        public static JsonFileTdcTagService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new JsonFileTdcTagService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Import the selected files into the json file
        /// </summary>
        public async Task<IEnumerable<TDCTag>> Import()
        {
            var filePathList = GetPathList();
            List<TDCTag> tagList = new();
            if (filePathList.Length == 0) return TDCTags;

            tagList = await ConvertFileToTagList(filePathList);

            // delete similar tags
            tagList = tagList.Distinct(new TDCTagComparer()).ToList();
            using StreamWriter writer = new(SavedFile);
            writer.Write(SerializeTagsList(tagList));

            TDCTags = tagList;
            return TDCTags;
        }

        /// <summary>
        /// Converted a file list to tag list
        /// </summary>
        /// <param name="filePathList">List of files to convert</param>
        /// <returns></returns>
        public async Task<List<TDCTag>> ConvertFileToTagList(string[] filePathList)
        {
            var dico = new Dictionary<string, Task<List<TDCTag>>>();
            foreach (string filePath in filePathList)
            {
                dico.Add(filePath, Task.Run(() =>
                {
                    var tdcFile = new TDCFileFactory(filePath).Create();

                    if (tdcFile is null)
                    {
                        return new List<TDCTag>();
                    }
                    return tdcFile.GetTagsList();
                }));
            }

            await Task.WhenAll(dico.Values).ConfigureAwait(false);

            var tagList = new List<TDCTag>();
            foreach (var item in dico)
            {
                tagList.AddRange(item.Value.Result);
            }

            return tagList;
        }

        public static string SerializeTagsList(IEnumerable<TDCTag> list)
        {
            return JsonSerializer.Serialize(list);
        }

        /// <summary>
        /// Open a fileDialog for import TDC Files
        /// </summary>
        /// <returns>List of TDC Files's path</returns>
        public static string[] GetPathList()
        {
            var pathList = Array.Empty<string>();
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

        public IEnumerable<TDCTag> GetAllListTag()
        {
            using StreamReader reader = new(SavedFile);
            var tags = JsonSerializer.Deserialize<List<TDCTag>>(reader.ReadToEnd());

            if (tags is null)
            {
                return new List<TDCTag>();
            }

            return tags;
        }

        public async Task<IEnumerable<TDCTag>> Search(string value)
        {
            return await Task.Run(() =>
            {
                Regex regex = new(value.RegexFormat());

                if (value != "")
                {
                    return (from tdcTag in TDCTags.AsParallel()
                            let matchName = regex.Matches(tdcTag.Name)
                            let matchValue = regex.Matches(tdcTag.Value)
                            where matchName.Count > 0 || matchValue.Count > 0
                            select tdcTag).ToList();
                }

                return TDCTags;
            }).ConfigureAwait(false);
        }

        public void InitializeJsonFile(string fileName)
        {
            SavedFile = fileName;
            if (File.Exists(SavedFile))
            {
                TDCTags = GetAllListTag().ToList();
            }
            else
            {
                using StreamWriter writer = new(SavedFile);
                writer.WriteLine("[]");
            }
        }
    }

    public sealed class LoadChangesMessage : ValueChangedMessage<bool>
    {
        public LoadChangesMessage(bool value) : base(value)
        {

        }
    }
}