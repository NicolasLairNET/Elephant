using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services.TDCFiles
{
    public class HWYFile : ITDCFile
    {
        public string[] FileContent { get; set; }

        public HWYFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            List<TDCTag> tagList = new();
            TDCTag tag = null;
            foreach (string line in FileContent)
            {
                if (line.Length > 3 && line[0..3] == "NET")
                {
                    tag = new()
                    {
                        Name = line[16..51].Trim(),
                        Parameter = "ENT_REF",
                        Value = line[52..line.Length].Trim(),
                        Origin = "HIWAY"
                    };
                    tagList.Add(tag);
                }
            }

            return tagList;
        }
    }
}
