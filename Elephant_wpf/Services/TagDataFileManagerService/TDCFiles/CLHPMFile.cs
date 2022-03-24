using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class CLHPMFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\s+UCN_CP\s.*\sPM\s.\sPMOD_ENT\s.*PM_SEQ\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public CLHPMFile(string filePath) : base(filePath) { }

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
                    var name = ColumnInfos.First(c => c.Name == "PMOD_ENT");
                    var valuePm = ColumnInfos.First(c => c.Name == "PM_SEQ");
                    var value = ColumnInfos.First(c => c.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tagPm = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(valuePm.StartIndex, valuePm.Length).Trim(),
                        Parameter = "PM_SEQ",
                        Origin = "CLHPM"
                    };

                    var tag = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "CLHPM"
                    };

                    tags.Add(tagPm);
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