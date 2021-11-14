using Elephant.DTOs;
using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class HMGRPFile : XXFile, ITDCFile
    {
        public string[] FileContent { get; set; }

        public HMGRPFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            var tagDto = new TagDto()
            {
                NamePosition = new int[2] { 52, 90 },
                Parameter = "ENT_REF",
                ValuePosition = new int[2] { 16, 51 },
                Origin = "HM GRP"
            };

            return CreateTagsList(FileContent, tagDto);
        }
    }
}
