using Elephant.Services.TagDataFileManagerService.TDCFiles;

namespace Elephant.Services.TagDataFileManagerService
{
    public class TDCFileFactory : Factory
    {
        public TDCFileFactory(string filePath) : base(filePath) { }

        public override ITDCFile? Create()
        {
            return FileExtension switch
            {
                ".EB" => new EBFile(FilePath),
                ".XX" => ReadCommand(FilePath) switch
                {
                    null => null,
                    var command when CDSFile.RegexCommand.IsMatch(command) => new CDSFile(FilePath),
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
}