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

            int[] namePosition = new int[2] { 16, 51 };
            string parameter = "CL";
            int[] valuePosition = new int[2] { 52, 60 };
            string origin = "CL AM";

            return CreateTagsList(
                FileContent, 
                namePosition, 
                parameter, 
                valuePosition, 
                origin);
        }
    }
}
