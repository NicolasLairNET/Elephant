using System;
using System.Collections.Generic;
using System.IO;

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
            string[] lines = File.ReadAllLines(FilePath);
            TDCTag tag = null;

            foreach (string line in lines)
            {
                if (line.Length > 3 && line[0..3] == "NET")
                {
                    if (FileName[0..3] == "UCN")
                    {
                        tag = CreateUCNTag(line);
                    }
                    else if (FileName[0..3] == "CLA")
                    {
                        tag = CreateCLAMTag(line);
                    }
                    pointList.Add(tag);
                }
            }
            

            return pointList;
        }

        private TDCTag CreateUCNTag(string line)
        {
            return new TDCTag()
            {
                Name = line[39..71].Trim(),
                Parameter = "ENT_REF",
                Value = line[72..104].Trim(),
                Origin = "UCN"
            };
        }

        private TDCTag CreateCLAMTag(string line)
        {
            return new TDCTag()
            {
                Name = line[17..35].Trim(),
                Parameter = "CL",
                Value = line[53..8].Trim(),
                Origin = "CL AM"
            };
        }
    }
}
