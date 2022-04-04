using Elephant_Models.Helpers;
using System.Text.RegularExpressions;

namespace Elephant.Model;

public class TagsFile
{
    public string Name { get; set; }
    public DateTime CreateAt { get; set; }
    public List<Tag> TagList { get; set; }

    public TagsFile()
    {
        Name = Environment.UserName;
        CreateAt = DateTime.Now;
        TagList = new List<Tag>();
    }

    /// <summary>
    /// Search in the list of tags
    /// </summary>
    /// <param name="value">Value to search</param>
    /// <returns>List of tags that matches the search</returns>
    public async Task<List<Tag>> Search(string value)
    {
        return await Task.Run(() =>
        {
            Regex regex = new(value.RegexFormat(), RegexOptions.IgnoreCase);

            if (value != "")
            {
                return (from tdcTag in TagList.AsParallel()
                        let matchName = regex.Matches(tdcTag.Name)
                        let matchValue = regex.Matches(tdcTag.Value)
                        where matchName.Count > 0 || matchValue.Count > 0
                        select tdcTag).ToList();
            }

            return TagList;
        });
    }
}
