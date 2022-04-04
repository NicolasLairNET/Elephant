using Elephant.Model;

namespace Elephant.Services.TagDataFile.FileType;
public class CLAMFile : XXFile, ITDCFile
{
    private const string PatternCommand = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s.*\sCL.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(PatternCommand, RegexOptions.Compiled);
    public CLAMFile(string filePath) : base(filePath) { }

    public List<Tag>? GetTagsList()
    {
        List<Tag> tags = new();

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
                    var valueCl = ColumnInfos.First(column => column.Name == "CL");

                    string lineCorrected = CorrectLineSize(line);

                    var tagCl = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(valueCl.StartIndex, valueCl.Length).Trim(),
                        Parameter = "CL",
                        Origin = "CLAM"
                    };

                    var tag = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "CLAM"
                    };

                    tags.Add(tagCl);
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