using System.Collections.Generic;

namespace ElephantLibrary
{
    internal class EBFile : ITDCFile
    {
        public List<TDCTag> Read(string[] lines)
        {
            List<TDCTag> pointList = new();
            string point = null;
            string parameter = null;
            string value = null;
            string origin = null;

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
