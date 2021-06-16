using System.Collections.Generic;
using System.IO;

namespace ElephantLibrary
{
    internal class EBFile : ITDCFile
    {

        public readonly string FilePath;

        public EBFile(string filePath)
        {
            FilePath = filePath;
        }

        public List<TDCTag> Read()
        {
            List<TDCTag> pointList = new();
            string point = null;
            string value = null;

            string[] lines = File.ReadAllLines(FilePath);

            foreach (string l in lines)
            {
                string line = l.Trim();
                if (line.Substring(0, 2) == "&N")
                {
                    continue;
                }

                if (line.Substring(0, 1) == "{")
                {
                    point = line[15..line.IndexOf('(')];
                    continue;
                }
                else if (line.Substring(0, 2) == "&T")
                {
                    value = line[3..];
                    TDCTag tag = new()
                    {
                        Name = point,
                        Parameter = "PNTTYPE",
                        Value = value,
                        Origin = "EB"
                    };
                    pointList.Add(tag);
                }
                else
                {
                    string[] element = line.Split("=");
                    TDCTag tag = new()
                    {
                        Name = point,
                        Parameter = element[0].Trim(),
                        Value = element[1].Replace("\"", "").Trim(),
                        Origin = "EB"
                    };
                    pointList.Add(tag);
                }
            }

            return pointList;
        }
    }
}
