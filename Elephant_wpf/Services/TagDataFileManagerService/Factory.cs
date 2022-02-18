using Elephant.Services.TagDataFileManagerService.TDCFiles;

namespace Elephant.Services.TagDataFileManagerService;

public abstract class Factory
{
    protected string FileName { get; set; }
    protected string FileExtension { get; set; }
    protected string FilePath { get; set; }

    protected Factory(string filePath)
    {
        FileName = Path.GetFileName(filePath);
        FileExtension = Path.GetExtension(filePath);
        FilePath = filePath;
    }

    public abstract ITDCFile? Create();
}

