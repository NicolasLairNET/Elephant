using Elephant.Model;
using Elephant_Services.ApplicationConfiguration;
using System.Text;

namespace Elephant_Services.Export;
public class ExportService : IExportService
{
    public IConfigFileService ConfigFileManager { get; set; }
    public ExportService(IConfigFileService configFileManager)
    {
        ConfigFileManager = configFileManager;
    }

    public async Task Export(List<Tag> tagList, string exportDestination)
    {
        var csvString = await GenerateCsvString(tagList).ConfigureAwait(false);
        await File.AppendAllTextAsync(exportDestination, csvString, Encoding.UTF8).ConfigureAwait(false);
    }

    private async Task<string> GenerateCsvString(List<Tag> tagList)
    {
        return await Task.Run(() =>
        {
            // create tags list with heading
            List<string> tags = new() { "Name", "Parameter", "Value", "Origin", Environment.NewLine };
            foreach (var tag in tagList)
            {
                tags.AddRange(tag.ToList());
            }

            return ConvertListToStringCsv(tags);
        });
    }

    private static string ConvertListToStringCsv(List<string> list)
    {
        return string.Join(",", list.ToArray()).Replace(Environment.NewLine + ",", Environment.NewLine);
    }
}
