using Elephant.Model;
using Elephant.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace Elephant.ViewModel;

public class TdcTagViewModel : ObservableObject
{
    private readonly IJsonTdcTagService JsonService;
    private readonly IExportService ExportService;
    private IEnumerable<TDCTag> tagsList;
    private string tagToSearch;
    public ICommand ImportCommand { get; }
    public ICommand ExportCommand { get; }
    public ICommand SearchCommand { get; }

    public IEnumerable<TDCTag> TagsList
    {
        get => tagsList;
        set => SetProperty(ref tagsList, value);
    }

    public TdcTagViewModel(IExportService exportService, IJsonTdcTagService jsonService)
    {
        ImportCommand = new AsyncRelayCommand(Import);
        ExportCommand = new AsyncRelayCommand(Export);
        SearchCommand = new AsyncRelayCommand(Search);

        JsonService = jsonService;
        ExportService = exportService;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "DATA.json");
        JsonService.InitializeJsonFile(path);
        tagsList = JsonService.TDCTags;
        tagToSearch = "";
    }

    public string TagToSearch
    {
        get => tagToSearch;
        set
        {
            tagToSearch = value;
            SearchCommand.Execute(null);
        }
    }

    private async Task Import()
    {
        TagsList = await JsonService.Import().ConfigureAwait(false);
    }

    private async Task Search()
    {
        TagsList = await JsonService.Search(TagToSearch).ConfigureAwait(false);
    }

    private async Task Export()
    {
        await ExportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }
}

