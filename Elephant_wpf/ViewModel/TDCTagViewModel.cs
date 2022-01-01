using Elephant.Model;
using Elephant.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace Elephant.ViewModel;

internal class TdcTagViewModel : ObservableObject
{
    private readonly IJsonTdcTagService JsonService;
    private readonly IExportService ExportService;
    private IEnumerable<TDCTag> tagsList;
    private string progressBarVisible = "Hidden";
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
        JsonService = jsonService;
        ExportService = exportService;
        ImportCommand = new RelayCommand(Import);
        ExportCommand = new RelayCommand(Export);
        SearchCommand = new RelayCommand(Search);
        string path = Path.Combine(Directory.GetCurrentDirectory(), "DATA.json");
        JsonService.InitializeJsonFile(path);
        TagsList = JsonService.TDCTags;
    }

    public string TagToSearch
    {
        get => tagToSearch;
        set
        {
            tagToSearch = value;
            Search();
        }
    }
    public string ProgressBarVisible
    {
        get => progressBarVisible;
        set => SetProperty(ref progressBarVisible, value);
    }

    private async void Import()
    {
        ProgressBarVisible = "Visible";
        TagsList = await JsonService.Import();
        ProgressBarVisible = "Hidden";
    }

    private async void Search()
    {
        TagsList = await JsonService.Search(TagToSearch);
    }

    private async void Export()
    {
        await ExportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }
}

