using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;
public class CLAMFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s.*\sCL.*\sENT_REF\s.*";
    public CLAMFile(string filePath) : base(filePath){}

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
                    var valueCl = ColumnInfos.First(column => column.Name == "CL");

                    string lineCorrected = CorrectLineSize(line);

                    var tagCl = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(valueCl.StartIndex, valueCl.Length).Trim(),
                        Parameter = "CL",
                        Origin = "CLAM"
                    };

                    var tag = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length),
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