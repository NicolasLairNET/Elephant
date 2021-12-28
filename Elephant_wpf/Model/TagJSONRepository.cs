using Elephant.Helpers;

namespace Elephant.Model;

public class TagJSONRepository
{
    /// <summary>
    /// JSON file path.
    /// </summary>
    public readonly string SavedFile;

    /// <summary>
    /// Construct tag handler through the name of json file.
    /// </summary>
    /// <param name="fileName">JSON file name.</param>
    public TagJSONRepository(string fileName)
    {
        SavedFile = fileName;
        InitializeJsonFile();
    }

    public IEnumerable<TDCTag> GetAllListTag()
    {
        using StreamReader reader = new(SavedFile);
        var tags = JsonSerializer.Deserialize<List<TDCTag>>(reader.ReadToEnd());

        return tags;
    }

    public async Task<IEnumerable<TDCTag>> Search(string value)
    {
            return  await Task.Run(() =>
            {
                Regex regex = new(value.RegexFormat());
                var newResult = GetAllListTag();

                if (value != "")
                {
                    newResult = (from tdcTag in newResult.AsParallel()
                                 let matchName = regex.Matches(tdcTag.Name)
                                 let matchValue = regex.Matches(tdcTag.Value)
                                 where matchName.Count > 0 || matchValue.Count > 0
                                 select tdcTag);
                }

                return newResult;
            }).ConfigureAwait(false);
    }

    private void InitializeJsonFile()
    {
        if (File.Exists(SavedFile)) return;

        using StreamWriter writer = new(SavedFile);
        writer.WriteLine("[]");
    }
}
