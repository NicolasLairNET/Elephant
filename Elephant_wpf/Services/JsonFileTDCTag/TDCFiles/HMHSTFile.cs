using Elephant.DTOs;
using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class HMHSTFile : XXFile, ITDCFile
    {
        public string[] FileContent { get; set; }

        public HMHSTFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            var tagDto = new TagDto()
            {
                NamePosition = new int[2] { 22, 60 },
                Parameter = "ENT_REF",
                ValuePosition = new int[2] { 16, 21 },
                Origin = "HM HST"
            };

            return CreateTagsList(FileContent, tagDto);
        }
    }
}
