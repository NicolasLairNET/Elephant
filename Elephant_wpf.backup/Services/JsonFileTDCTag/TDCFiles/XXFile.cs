using Elephant.DTOs;
using Elephant.Model;
using System.Collections.Generic;

namespace Elephant.Services
{
    public abstract class XXFile
    {
        /// <summary>
        /// Create a TDCTag list with the content of the file.
        /// </summary>
        /// <param name="fileContent">Content of the file</param>
        /// <param name="tagDto"></param>
        /// <returns>The list of TDCTags in the file </returns>
        public List<TDCTag> CreateTagsList(string[] fileContent,TagDto tagDto)
        {
            List<TDCTag> tagList = new();
            TDCTag tag = null;
            foreach (string line in fileContent)
            {
                if (line.Length > 3 && line[0..3] == "NET")
                {
                    tag = new()
                    {
                        Name = line[tagDto.NamePosition[0]..tagDto.NamePosition[1]].Trim(),
                        Parameter = tagDto.Parameter,
                        Value = line[tagDto.ValuePosition[0]..tagDto.ValuePosition[1]].Trim(),
                        Origin = tagDto.Origin
                    };
                    tagList.Add(tag);
                }
            }

            return tagList;
        }
    }
}
