using System.ComponentModel;
using Elephant.Model;
using Elephant.Services;
using Elephant.Services.ExportService;

namespace Elephant.ViewModel;

class TdcTagViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly JsonFileTdcTagService _jsonFileTdcTagService;
    private readonly ExportService _exportService;
    private string _tagToSearch;
    private ObservableCollection<TDCTag> _tagsList;

    public Command ImportCommand { get; init; }
    public Command ExportCommand { get;  init; }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<TDCTag> TagsList
    {
        get => _tagsList;
        set
        {
            _tagsList = value;
            OnPropertyChanged(nameof(TagsList));
        }
    }

    public string TagToSearch
    {
        get => _tagToSearch;
        set
        {
            _tagToSearch = value;
            Search();
        }
    }

    public TdcTagViewModel()
    {
        _jsonFileTdcTagService = new JsonFileTdcTagService();
        _exportService = new ExportService();
        ImportCommand = new Command(Import);
        ExportCommand = new Command(Export);
        TagsList = _jsonFileTdcTagService.GetTDCTags();
    }

    private void Import()
    {
        TagsList = _jsonFileTdcTagService.Import();
    }

    private void Search()
    {
        TagsList = _jsonFileTdcTagService.Search(TagToSearch);
    }

    private async void Export()
    {
        var tags = new List<TDCTag>(TagsList);
        await _exportService.Export(tags).ConfigureAwait(false);
    }
}

