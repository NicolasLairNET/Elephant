using Elephant.Services.JsonFileTDCTag.TDCFiles;

namespace Elephant.Services;

public class TDCFileFactory : Factory
{
    public TDCFileFactory(string filePath) : base(filePath)
    {
    }

    public override ITDCFile? Create()
    {
        return FileExtension switch
        {
            ".EB" => new EBFile(FilePath),
            ".XX" => FileName switch
            {
                _ when FileName.Contains("UCN") => new UCNFile(FilePath),
                _ when FileName.Contains("HIWAY") => new HWYFile(FilePath),
                _ when FileName.Contains("CLAM") => new CLAMFile(FilePath),
                _ when FileName.Contains("CLHPM") => new CLHPMFile(FilePath),
                _ when FileName.Contains("CDS") => new CDSFile(FilePath),
                _ when FileName.Contains("PE") => new PEFile(FilePath),
                _ when FileName.Contains("HMGRP") => new HMGRPFile(FilePath),
                _ when FileName.Contains("HMHST") => new HMHSTFile(FilePath),
                _ => null
            },
            _ => null
        };
    }
}

