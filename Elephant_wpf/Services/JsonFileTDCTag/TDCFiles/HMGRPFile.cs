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
            int[] namePosition = new int[2] { 52, 90 };
            string parameter = "ENT_REF";
            int[] valuePosition = new int[2] { 16, 51 };
            string origin = "HM GRP";

            return CreateTagsList(
                FileContent,
                namePosition,
                parameter,
                valuePosition,
                origin);
        }
    }
}
