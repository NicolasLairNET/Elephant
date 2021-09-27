using Elephant.Model;
using System.Collections.Generic;

namespace Elephant.Services.JsonFileTDCTag
{
    public abstract class XXFile
    {
        /// <summary>
        /// Create a TDCTag list with the content of the file.
        /// </summary>
        /// <param name="fileContent">Content of the file</param>
        /// <param name="namePosition">Position of the name in the line. index 0 represents the beginning position of the name and index 1 the end.</param>
        /// <param name="parameter">Parameter name</param>
        /// <param name="valuePosition">Position of the value in the line. index 0 represents the beginning position of the value and index 1 the end.</param>
        /// <param name="origin">Origin name</param>
        /// <returns>The list of TDCTags in the file </returns>
        public List<TDCTag> CreateTagsList(string[] fileContent,
            int[] namePosition, 
            string parameter, 
            int[] valuePosition, 
            string origin)
        {
            List<TDCTag> tagList = new();
            TDCTag tag = null;
            foreach (string line in fileContent)
            {
                if (line.Length > 3 && line[0..3] == "NET")
                {
                    tag = new()
                    {
                        Name = line[namePosition[0]..namePosition[1]].Trim(),
                        Parameter = parameter,
                        Value = line[valuePosition[0]..valuePosition[1]].Trim(),
                        Origin = origin
                    };
                    tagList.Add(tag);
                }
            }

            return tagList;
        }
    }
}
