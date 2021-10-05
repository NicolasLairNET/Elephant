using System.IO;

namespace Elephant.Services
{
    public abstract class Factory
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }

        public Factory(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FileExtension = Path.GetExtension(filePath);
            FilePath = filePath;
        }

        public abstract ITDCFile Create();
    }
}
