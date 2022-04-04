using Elephant_Services.TagDataFile.FileType;

namespace Elephant_Services.TagDataFile;

public class TagFileFactory : Factory
{
    public TagFileFactory(string filePath) : base(filePath) { }

    public override ITDCFile? Create()
    {
        return FileExtension switch
        {
            ".EB" => new EBFile(FilePath),
            ".XX" => ReadCommand(FilePath) switch
            {
                null => null,
                var command when CdsFile.RegexCommand.IsMatch(command) => new CdsFile(FilePath),
                var command when UCNFile.RegexCommand.IsMatch(command) => new UCNFile(FilePath),
                var command when HWYFile.RegexCommand.IsMatch(command) => new HWYFile(FilePath),
                var command when CLAMFile.RegexCommand.IsMatch(command) => new CLAMFile(FilePath),
                var command when CLHPMFile.RegexCommand.IsMatch(command) => new CLHPMFile(FilePath),
                var command when PEFile.RegexCommand.IsMatch(command) => new PEFile(FilePath),
                var command when HMHSTFile.RegexCommand.IsMatch(command) => new HMHSTFile(FilePath),
                var command when HMGRPFile.RegexCommand.IsMatch(command) => new HMGRPFile(FilePath),
                _ => null
            },
            _ => null
        };
    }

    private string? ReadCommand(string filePath)
    {
        var fileContent = File.ReadAllLines(filePath);
        foreach (string line in fileContent)
        {
            if (line.Contains("FN"))
            {
                return line;
            }
        }

        return null;
    }
}
