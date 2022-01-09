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
    private bool isLoading;

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
        tagsList = JsonService.TDCTags;
        tagToSearch = "";
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

    public bool IsLoading
    {
        get => isLoading;
        set
        {
            SetProperty(ref isLoading, value);
        }
    }

    private async void Import()
    {
        TagsList = await JsonService.Import(this);
    }

    private async void Search()
    {
        TagsList = await JsonService.Search(TagToSearch).ConfigureAwait(false);
    }

    private async void Export()
    {
        await ExportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }
}

