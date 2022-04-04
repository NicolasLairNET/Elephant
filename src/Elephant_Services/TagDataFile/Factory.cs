using Elephant_Services.TagDataFile.FileType;

namespace Elephant_Services.TagDataFile;

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

