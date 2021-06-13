using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ElephantLibrary
{
    public class ElephantJson
    {
        public List<Point> SearchDataJson(string pointName)
        {
            string data = File.ReadAllText(@"D:\Nico\Nico\DATA.json");
            var pointList = JsonSerializer.Deserialize<List<Point>>(data);

            List<Point> result = pointList.FindAll(
                delegate (Point p)
                {
                    return p.Name == pointName || p.Value == pointName;
                }
            );

            return result;
        }
    }
}
