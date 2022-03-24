using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HWYFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\s+HWY_CP\s.*\sENTITY\s.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public HWYFile(string filePath) : base(filePath) { }

    public List<TDCTag>? GetTagsList()
    {
        List<TDCTag> tags = new();

        if (ColumnInfos == null)
        {
            return null;
        }

        foreach (string line in FileContent)
        {
            try
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(column => column.Name == "ENTITY");
                    var value = ColumnInfos.First(column => column.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "HIWAY"
                    };
                    tags.Add(tag);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Erreur de lecture du fichier {FileName}");
                return null;
            }
        }

        return tags;
    }
}