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
                if (line.Contains("NET"))
                {
                    var pe = ColumnInfos.First(c => c.Name == "PE");
                    var ent_ref = ColumnInfos.First(c => c.Name == "ENT_REF");

                    string value = line.Substring(ent_ref.StartIndex);

                    if (value.Length == ent_ref.Length)
                    {
                        value = value[..ent_ref.Length];
                    }

                    var tag = new TDCTag()
                    {
                        Name = line.Substring(pe.StartIndex, pe.Length),
                        Value = value.Trim(),
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