using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class XXFile : ITDCFile
    {
        public readonly string FileName;
        public readonly string[] FileContent;

        public XXFile(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> Read()
        {
            List<TDCTag> pointList = new();
            TDCTag tag = null;

            foreach (string line in FileContent)
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
                    else if (FileName[0..5] == "HIWAY")
                    {
                        tag = CreateHWYTag(line);
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
