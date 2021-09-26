using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services.TDCFiles
{
    public class CLAMFile : ITDCFile
    {
        public string[] FileContent { get; set; }

        public CLAMFile(string filePath)
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
                        Parameter = "CL",
                        Value = line[52..60].Trim(),
                        Origin = "CL AM"
                    };
                    tagList.Add(tag);
                }
            }

            return tagList;
        }
    }
}
