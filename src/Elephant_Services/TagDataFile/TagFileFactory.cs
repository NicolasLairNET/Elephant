using Elephant_Services.TagDataFile.FileType;

namespace Elephant_Services.TagDataFile;

public class TagFileFactory
{
    public ITDCFile? Create(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);
        return fileExtension switch
        {
            ".EB" => new EBFile(filePath),
            ".XX" => ReadCommand(filePath) switch
            {
                null => null,
                var command when CdsFile.RegexCommand.IsMatch(command) => new CdsFile(filePath),
                var command when UCNFile.RegexCommand.IsMatch(command) => new UCNFile(filePath),
                var command when HWYFile.RegexCommand.IsMatch(command) => new HWYFile(filePath),
                var command when CLAMFile.RegexCommand.IsMatch(command) => new CLAMFile(filePath),
                var command when CLHPMFile.RegexCommand.IsMatch(command) => new CLHPMFile(filePath),
                var command when PEFile.RegexCommand.IsMatch(command) => new PEFile(filePath),
                var command when HMHSTFile.RegexCommand.IsMatch(command) => new HMHSTFile(filePath),
                var command when HMGRPFile.RegexCommand.IsMatch(command) => new HMGRPFile(filePath),
                _ => new InvalidFile(filePath)
            },
            _ => new InvalidFile(filePath)
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
