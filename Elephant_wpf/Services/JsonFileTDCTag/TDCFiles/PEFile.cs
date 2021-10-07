using Elephant.Model;
using System.Collections.Generic;
using System.IO;

namespace Elephant.Services
{
    public class UCNFile : XXFile , ITDCFile
    {
        public string[] FileContent { get; set; }

        public UCNFile(string filePath)
        {
            FileContent = File.ReadAllLines(filePath);
        }

        public List<TDCTag> GetTagsList()
        {
            int[] namePosition = new int[2] { 39, 71 };
            string parameter = "ENT_REF";
            int[] valuePosition = new int[2] { 72, 104 };
            string origin = "UCN";

            return CreateTagsList(
                FileContent, 
                namePosition, 
                parameter, 
                valuePosition, 
                origin);
        }
    }
}
