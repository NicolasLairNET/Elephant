using System.Windows.Input;
using Elephant.Model;
using Elephant.Services;
using Elephant.Services.ExportService;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Elephant.ViewModel;

class TdcTagViewModel : ObservableObject
{
    private readonly JsonFileTdcTagService jsonFileTdcTagService;
    private readonly ExportService exportService;
    private string tagToSearch;
    private ObservableCollection<TDCTag> tagsList;

    public ICommand ImportCommand { get; }
    public ICommand ExportCommand { get; }

    public ObservableCollection<TDCTag> TagsList
    {
        get => tagsList;
        set => SetProperty(ref tagsList, value);
    }
    public TdcTagViewModel()
    {
        jsonFileTdcTagService = new JsonFileTdcTagService();
        exportService = new ExportService();
        ImportCommand = new RelayCommand(Import);
        ExportCommand = new RelayCommand(Export);
        TagsList = jsonFileTdcTagService.GetTDCTags();
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

    private void Import()
    {
        TagsList = jsonFileTdcTagService.Import();
    }

    private void Search()
    {
        TagsList = jsonFileTdcTagService.Search(TagToSearch);
    }

    private async void Export()
    {
        var tags = new List<TDCTag>(TagsList);
        await exportService.Export(tags).ConfigureAwait(false);
    }
}

