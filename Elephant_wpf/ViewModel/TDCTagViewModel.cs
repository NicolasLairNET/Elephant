using System.Windows.Input;
using Elephant.Model;
using Elephant.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Elephant.ViewModel;

internal class TdcTagViewModel : ObservableObject
{
    private readonly IJsonTdcTagService JsonService;
    private readonly IExportService ExportService;
    private string tagToSearch;
    private List<TDCTag> tagsList;

    public ICommand ImportCommand { get; }
    public ICommand ExportCommand { get; }

    public List<TDCTag> TagsList
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
        TagsList = JsonService.GetTDCTags();
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
        TagsList = JsonService.Import();
    }

    private void Search()
    {
        TagsList = JsonService.Search(TagToSearch);
    }

    private async void Export()
    {
        await ExportService.Export(TagsList).ConfigureAwait(false);
    }
}

