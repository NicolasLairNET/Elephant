using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ElephantLibrary
{
    public static class DataFile
    {
        private static string DataPath => $"{Directory.GetCurrentDirectory()}\\DATA.json";

        public static void CreateDataFile(string[] fileList)
        {
            List<TDCTag> pointList = new();
            string[] fileLines;

            foreach (string fileName in fileList)
            {
                fileLines = File.ReadAllLines(fileName);
                EBFile eb = new();
                List<TDCTag> EBList = eb.Read(fileLines);
                pointList.AddRange(EBList);
            }
            string result = JsonSerializer.Serialize(pointList);
            File.WriteAllText(DataPath, result);
        }


        public static List<TDCTag> Search(string tagName)
        {
            string data = File.ReadAllText(DataPath);
            List<TDCTag> pointList = JsonSerializer.Deserialize<List<TDCTag>>(data);

            List<TDCTag> result = pointList.FindAll(
                delegate (TDCTag p)
                {
                    return p.Name == tagName || p.Value == tagName;
                }
            );

            return result;
        }

    }
}
