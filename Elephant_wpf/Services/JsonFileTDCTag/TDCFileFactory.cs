namespace Elephant.Services.JsonFileTDCTag
{
    public class TDCFileFactory : Factory
    {
        public TDCFileFactory(string filePath) : base(filePath)
        {
        }

        public override ITDCFile Create()
        {
            if (FileExtension == ".EB")
            {
                return new EBFile(FilePath);
            }
            else if (FileExtension == ".XX")
            {
                return FileName switch
                {
                    _ when FileName.Contains("UCN") => new UCNFile(FilePath),
                    _ when FileName.Contains("HIWAY") => new HWYFile(FilePath),
                    _ when FileName.Contains("CLAM") => new CLAMFile(FilePath),
                    _ when FileName.Contains("CLHPM") => new CLHPMFile(FilePath),
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }
    }


}
