using Elephant.Model;

namespace Elephant.Services.JsonFileTDCTag.TDCFiles;

class EBFile : ITDCFile
{
    public string[] FileContent { get; }

    public EBFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        List<TDCTag> tagsList = new();
        string point = null;
        string value = null;

        foreach (string l in FileContent)
        {
            string line = l.Trim();
            if (line?.Length == 0 || line[0..2] == "&N")
            {
                continue;
            }

            if (line.Substring(0, 1) == "{")
            {
                point = line[15..line.IndexOf('(')];
                continue;
            }
            else if (line[0..2] == "NN")
            {
                string[] parameters = line.Split("  ");
                foreach (string parameter in parameters)
                {
                    var tag = ReadParameter(parameter, point);
                    if (tag != null)
                    {
                        tagsList.Add(tag);
                    }
                }
            }
            else if (line[0..2] == "&T")
            {
                value = line[3..];
                if (point != "" && value != "")
                {
                    TDCTag tag = new()
                    {
                        Name = point,
                        Parameter = "PNTTYPE",
                        Value = value,
                        Origin = "EB"
                    };
                    tagsList.Add(tag);
                }
            }
            else
            {
                var tag = ReadParameter(line, point);
                if (tag != null)
                {
                    tagsList.Add(tag);
                }
            }
        }

        return tagsList;
    }

    private TDCTag ReadParameter(string line, string point)
    {
        if (point != "" && line.Contains('='))
        {
            string[] element = line.Split("=");
            TDCTag tag = new()
            {
                Name = point,
                Parameter = element[0].Trim(),
                Value = element[1].Replace("\"", "").Trim(),
                Origin = "EB"
            };

            return tag;
        }
        return null;
    }
}

