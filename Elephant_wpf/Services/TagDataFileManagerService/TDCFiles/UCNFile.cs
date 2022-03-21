using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class UCNFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\sUCN_CP\s.*\sNODE_CP\s.*\sMODULE\s.*\sSLOT\s.\sENTITY\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public UCNFile(string filePath) : base(filePath) { }

    public List<TDCTag>? GetTagsList()
    {
        try
        {
            List<TDCTag> tags = new();

            if (ColumnInfos == null)
            {
                return tags;
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
                        Origin = "UCN"
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