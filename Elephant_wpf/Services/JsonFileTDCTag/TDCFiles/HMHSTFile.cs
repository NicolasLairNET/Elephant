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
            int[] namePosition = new int[2] { 22, 60 };
            string parameter = "ENT_REF";
            int[] valuePosition = new int[2] { 16, 21 };
            string origin = "HM HST";

            return CreateTagsList(
                FileContent,
                namePosition,
                parameter,
                valuePosition,
                origin);
        }
    }
}
