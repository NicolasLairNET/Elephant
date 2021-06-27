using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElephantLibrary
{
    class XXFile : ITDCFile
    {
        public readonly string FilePath;
        public readonly string FileName;

        public XXFile(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
        }

        public List<TDCTag> Read()
        {
            List<TDCTag> pointList = new();

            if (FileName[0..3] == "UCN")
            {
                pointList = ReadUCN();
            }

            return pointList;
        }

        private List<TDCTag> ReadUCN()
        {
            List<TDCTag> pointList = new();
            string[] lines = File.ReadAllLines(FilePath);

            foreach (string line in lines)
            {
                if (line.Length > 3 && line[0..3] == "NET")
                {
                    TDCTag tag = new()
                    {
                        Name = line[39..71].Trim(),
                        Parameter = "ENT_REF",
                        Value = line[72..104].Trim(),
                        Origin = "UCN"
                    };

                    pointList.Add(tag);
                }
            }

            return pointList;
        }
    }
}
