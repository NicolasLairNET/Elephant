using Elephant.Model;
using Elephant.Services.ConfigFileManagerService;
using System.Windows.Forms;

namespace Elephant.Services.ExportService;
public class ExportService : IExportService
{
    public IConfigFileManagerService ConfigFileManager { get; set; }
    public ExportService(IConfigFileManagerService configFileManager)
    {
        ConfigFileManager = configFileManager;
    }

    public async Task Export(List<TDCTag> tagList)
    {
        var path = SelectPathExport();
        if (path is null) return;

        var csvString = await GenerateCsvString(tagList).ConfigureAwait(false);
        await File.AppendAllTextAsync(path, csvString, Encoding.UTF8).ConfigureAwait(false);

        System.Windows.MessageBox.Show($"Export terminé dans {path}");
    }

    private async Task<string> GenerateCsvString(List<TDCTag> tagList)
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

    private string? SelectPathExport()
    {
        var defaultFileName = $"export{DateTime.Now:ddMMyyyyHmmss}.csv";
        var defaultPath = ConfigFileManager.ExportFilePath;

        SaveFileDialog saveFileDialog = new();
        saveFileDialog.FileName = defaultFileName;
        saveFileDialog.DefaultExt = ".csv";
        saveFileDialog.InitialDirectory = defaultPath;

        return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : null;
    }
}
