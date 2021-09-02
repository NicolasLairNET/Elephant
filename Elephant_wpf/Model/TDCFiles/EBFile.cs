using System.Collections.Generic;
using System.IO;

namespace Elephant.Model
{
    internal class EBFile : ITDCFile
    {
        public readonly string FileName;
        public readonly string[] FileContent;

        public EBFile(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> Read()
        {
            List<TDCTag> pointList = new();
            string point = null;
            string value = null;

            foreach (string l in FileContent)
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
