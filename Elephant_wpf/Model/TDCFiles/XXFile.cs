using System.Collections.Generic;
using System.IO;

namespace Elephant.Model
{
    public class XXFile : ITDCFile
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
                    else if(FileName[0..5] == "HIWAY")
                    {
                        tag = CreateHWYTag(line);
                    }
                    pointList.Add(tag);
                }
            }
            
            return pointList;
        }

        public TDCTag CreateUCNTag(string line)
        {
            return new TDCTag()
            {
                Name = line[39..71].Trim(),
                Parameter = "ENT_REF",
                Value = line[74..106].Trim(),
                Origin = "UCN"
            };
        }

        private TDCTag CreateCLAMTag(string line)
        {
            return new TDCTag()
            {
                Name = line[17..35].Trim(),
                Parameter = "CL",
                Value = line[53..61].Trim(),
                Origin = "CL AM"
            };
        }

        private TDCTag CreateHWYTag(string line)
        {
            return new TDCTag()
            {
                Name = line[17..35].Trim(),
                Parameter = "ENT_REF",
                Value = line[53..38].Trim(),
                Origin = "HIWAY"
            };
        }
    }
}
