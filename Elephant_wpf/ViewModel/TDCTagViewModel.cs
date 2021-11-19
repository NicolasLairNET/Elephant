using System.ComponentModel;
using Elephant.Model;
using Elephant.Services;
using Elephant.Services.ExportService;
using Elephant.ViewModel.Commands;

namespace Elephant.ViewModel;

class TDCTagViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly JsonFileTdcTagService _jsonFileTdcTagService;
    private readonly ExportService _exportService;
    public SearchCommand SearchCommand { get; private set; }
    public ImportCommand ImportCommand { get; private set; }
    public ExportCommand ExportCommand { get; private set; }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public TDCTagViewModel()
    {
        _jsonFileTdcTagService = new JsonFileTdcTagService();
        _exportService = new ExportService();
        ImportCommand = new ImportCommand(Import);
        SearchCommand = new SearchCommand(Search);
        ExportCommand = new ExportCommand(Export);
        TagsList = _jsonFileTdcTagService.GetTDCTags();
    }

    private void Import()
    {
        TagsList = _jsonFileTdcTagService.Import();
    }

    private void Search(string tagName)
    {
        TagsList = _jsonFileTdcTagService.Search(tagName);
    }

    private void Export()
    {
        var tags = new List<TDCTag>(TagsList);
        _exportService.Export(tags);
    }

    private ObservableCollection<TDCTag> _tagsList;
    public ObservableCollection<TDCTag> TagsList
    {
        get => _tagsList;
        set
        {
            _tagsList = value;
            OnPropertyChanged(nameof(TagsList));
        }
    }
}

