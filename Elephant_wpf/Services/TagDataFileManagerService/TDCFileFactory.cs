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
                    var command when CheckCommand(command, CDSFile.CommandRegex) => new CDSFile(FilePath),
                    var command when CheckCommand(command, UCNFile.CommandRegex) => new UCNFile(FilePath),
                    var command when CheckCommand(command, HWYFile.CommandRegex) => new HWYFile(FilePath),
                    var command when CheckCommand(command, CLAMFile.CommandRegex) => new CLAMFile(FilePath),
                    var command when CheckCommand(command, CLHPMFile.CommandRegex) => new CLHPMFile(FilePath),
                    var command when CheckCommand(command, PEFile.CommandRegex) => new PEFile(FilePath),
                    var command when CheckCommand(command, HMHSTFile.CommandRegex) => new HMHSTFile(FilePath),
                    var command when CheckCommand(command, HMGRPFile.CommandRegex) => new HMGRPFile(FilePath),
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

        private bool CheckCommand(string command, string regex)
        {
            return new Regex(regex).IsMatch(command);
        }
    }
}