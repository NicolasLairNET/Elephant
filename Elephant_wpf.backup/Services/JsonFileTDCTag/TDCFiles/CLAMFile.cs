using Elephant.DTOs;
using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class CLAMFile : XXFile, ITDCFile
    {
        public string[] FileContent { get; set; }

        public CLAMFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            var tagDto = new TagDto()
            { 
                NamePosition = new int[2] { 16, 51 },
                Parameter = "CL",
                ValuePosition = new int[2] { 52, 60 },
                Origin = "CL AM"
            };

            return CreateTagsList(FileContent, tagDto);
        }
    }
}
