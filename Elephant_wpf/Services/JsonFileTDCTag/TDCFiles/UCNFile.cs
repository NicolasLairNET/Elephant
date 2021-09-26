using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class UCNFile : ITDCFile
    {
        public string[] FileContent { get; set; }

        public UCNFile(string filePath)
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
                        Name = line[39..71].Trim(),
                        Parameter = "ENT_REF",
                        Value = line[72..104].Trim(),
                        Origin = "UCN"
                    };
                    tagList.Add(tag);
                }
            }

            return tagList;
        }
    }
}
