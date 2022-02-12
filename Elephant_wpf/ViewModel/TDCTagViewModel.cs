using Elephant.Messages;
using Elephant.Model;
using Elephant.Services.ExportService;
using Elephant.Services.JsonFileTDCTag;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant.ViewModel;

public class TdcTagViewModel : ObservableRecipient, IViewModel
{
    private readonly IJsonTdcTagService _jsonTdcTagService;
    private readonly IExportService _exportService;
    private List<TDCTag> _tagsList;
    private string _tagToSearch;
    private int _numberFilesImported;
    private int _totalFilesToImport;
    private string _importMessage;
    private string _importFile;
    public IAsyncRelayCommand ImportCommand { get; }
    public IAsyncRelayCommand ExportCommand { get; }
    public IAsyncRelayCommand SearchCommand { get; }
    public IRelayCommand UpdateViewCommand { get; }


    public TdcTagViewModel(IExportService exportService, IJsonTdcTagService jsonService)
    {
        ImportCommand = new AsyncRelayCommand(Import);
        ExportCommand = new AsyncRelayCommand(Export);
        SearchCommand = new AsyncRelayCommand(Search);

        _jsonTdcTagService = jsonService;
        _exportService = exportService;
        _tagsList = _jsonTdcTagService.TDCTags;
        _tagToSearch = "";
        _numberFilesImported = 0;
        _totalFilesToImport = 0;
        _importMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
        _importFile = "";

        UpdateViewCommand = new RelayCommand(SendMessage);
        OnActivated();
    }
    public List<TDCTag> TagsList
    {
        get => _tagsList;
        set => SetProperty(ref _tagsList, value);
    }

    public string TagToSearch
    {
        get => _tagToSearch;
        set
        {
            _tagToSearch = value;
            SearchCommand.Execute(null);
        }
    }

    public string ImportMessage
    {
        get => _importMessage;
        set
        {
            SetProperty<string>(ref _importMessage, value);
        }
    }

    public string ImportFile
    {
        get => _importFile;
        set
        {
            SetProperty(ref _importFile, value);
        }
    }

    private async Task Import()
    {
        var filePathList = _jsonTdcTagService.GetPathList();
        if (filePathList != null)
        {
            InitializeImport(filePathList.Length);

            var tasks = new List<Task>();
            Progress<(string, List<TDCTag>)> p = new();
            p.ProgressChanged += (_, args) =>
            {
                TagsList.AddRange(args.Item2);
                _numberFilesImported++;
                ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
                ImportFile = args.Item1;
            };

            foreach (string filePath in filePathList)
            {
                tasks.Add(_jsonTdcTagService.GetTagsAsync(filePath, p));
            }

            await Task.WhenAll(tasks);

            TagsList = TagsList.Distinct().ToList();
            _jsonTdcTagService.WriteData(TagsList);
            _jsonTdcTagService.TDCTags = TagsList;
        }
    }

    private void InitializeImport(int numberFile)
    {
        TagsList.Clear();
        _numberFilesImported = 0;
        _totalFilesToImport = numberFile;
        ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
    }

    private async Task Search()
    {
        TagsList = await _jsonTdcTagService.Search(TagToSearch).ConfigureAwait(false);
    }

    private async Task Export()
    {
        await _exportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }

    public void SendMessage()
    {
        Messenger.Send(new ViewModelChangedMessage("ParameterViewModel"));
    }

    /// <summary>
    /// Activates the listening of the message which notifies the change of the data file.
    /// When the file is changed the list of tags is updated.
    /// </summary>
    protected override void OnActivated()
    {
        Messenger.Register<TdcTagViewModel, DataFileChangedMessage>(this, (r, m) =>
        {
            r.TagsList = _jsonTdcTagService.GetTagsToDataFile(m.Value);
            // update tags list in the jsonTdcfileService
            _jsonTdcTagService.TDCTags = r.TagsList.ToList();
        });
    }
}