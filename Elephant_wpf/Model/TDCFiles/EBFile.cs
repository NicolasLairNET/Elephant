using System.Collections.Generic;
using System.IO;

namespace Elephant.Model
{
    internal class EBFile : ITDCFile
    {

        public readonly string FilePath;
        public readonly string FileName;

        public EBFile(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
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
                if (line[0..2] == "&N")
                {
                    continue;
                }

                if (line.Substring(0, 1) == "{")
                {
                    point = line[15..line.IndexOf('(')];
                    continue;
                }
                else if (line[0..2] == "NN")
                {
                    string[] parameters = line.Split("  ");
                    foreach (string parameter in parameters)
                    {
                        pointList.Add(ReadParameter(parameter, point));
                    }
                }
                else if (line[0..2] == "&T")
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
                    pointList.Add(ReadParameter(line, point));
                }
            }

            return pointList;
        }

        private static TDCTag ReadParameter(string line, string point)
        {
            string[] element = line.Split("=");
            TDCTag tag = new()
            {
                Name = point,
                Parameter = element[0].Trim(),
                Value = element[1].Replace("\"", "").Trim(),
                Origin = "EB"
            };

            return tag;
        }
    }
}
