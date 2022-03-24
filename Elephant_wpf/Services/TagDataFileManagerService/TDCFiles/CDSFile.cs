using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class CDSFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s(?:(?!\bCL\b).)*\sENT_REF\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public CDSFile(string filePath) : base (filePath) {}

    public List<TDCTag>? GetTagsList()
    {
        try
        {
            List<TDCTag> tags = new();

            if (ColumnInfos == null)
            {
                return null;
            }

            foreach (var line in FileContent)
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
                        Origin = "CDS"
                    };
                    tags.Add(tag);
                }
            }

            return tags;
        }
        catch (Exception)
        {
            System.Windows.MessageBox.Show($"Erreur de lecture du fichier {FileName}");
            return null;
        }
    }
}
