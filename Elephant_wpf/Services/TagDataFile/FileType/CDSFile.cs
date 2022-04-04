using Elephant.Model;

namespace Elephant.Services.TagDataFile.FileType;

public class CdsFile : XXFile, ITDCFile
{
    private const string PatternCommand = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s(?:(?!\bCL\b).)*\sENT_REF\s.*";
    public static Regex RegexCommand = new(PatternCommand, RegexOptions.Compiled);
    public CdsFile(string filePath) : base(filePath) { }

    public List<Tag>? GetTagsList()
    {
        try
        {
            List<Tag> tags = new();

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

                    var tag = new Tag()
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
