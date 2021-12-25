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
    private readonly TagJSONRepository _repository;
    private IEnumerable<TDCTag> tagsList;

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
        _repository = new TagJSONRepository(path);
        TagsList = _repository.GetAllListTag();
    }

    public string TagToSearch { get; set; }

    private void Import()
    {
        JsonService.Import(_repository.SavedFile);
        TagsList = _repository.GetAllListTag();
    }

    private async void Search()
    {
        TagsList = await _repository.Search(TagToSearch).ConfigureAwait(false);
    }

    private async void Export()
    {
        await ExportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }
}

