using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class CLHPMFile : XXFile, ITDCFile
    {
        public string[] FileContent { get; set; }

        public CLHPMFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            int[] namePosition = new int[2] { 20, 36 };
            string parameter = "ENT_REF";
            int[] valuePosition = new int[2] { 46, 84 };
            string origin = "CLHPM";

            return CreateTagsList(
                FileContent,
                namePosition,
                parameter,
                valuePosition,
                origin);
        }
    }
}
