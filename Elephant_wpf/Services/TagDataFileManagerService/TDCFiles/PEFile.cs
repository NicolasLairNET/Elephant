using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class PEFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\s+PE\s.*\sENT_REF\s.*";
    public PEFile(string filePath) : base(filePath) { }

    public List<TDCTag>? GetTagsList()
    {
        try
        {
            List<TDCTag> tags = new();

            if (ColumnInfos == null)
            {
                return null;
            }

            foreach (string line in FileContent)
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(c => c.Name == "PE");
                    var value = ColumnInfos.First(c => c.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length),
                        Parameter = "ENT_REF",
                        Origin = "PE"
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