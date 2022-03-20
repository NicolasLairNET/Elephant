using Elephant.Messages;
using Elephant.Model;
using Elephant.Services.ConfigFileManagerService;
using Elephant.Services.ExportService;
using Elephant.Services.TagDataFileManagerService;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant.ViewModel;

public class TdcTagViewModel : ObservableRecipient, IViewModel
{
    private readonly ITagDataFileManager _tagDataFileManager;
    private readonly IExportService _exportService;
    private readonly IConfigFileManagerService _configFileManagerService;

    private List<TDCTag> _tagsDataGrid;
    private int _numberFilesImported = 0;
    private int _totalFilesToImport = 0;
    private string _tagToSearch = "";
    private string _importMessage = "";
    private string _importFile = "";
    private TagDataFile _tagDataFile;

    public IAsyncRelayCommand ImportCommand { get; }
    public IAsyncRelayCommand ExportCommand { get; }
    public IAsyncRelayCommand SearchCommand { get; }
    public IRelayCommand UpdateViewCommand { get; }

    public TdcTagViewModel(
        IExportService exportService,
        ITagDataFileManager tagDataFileManager,
        IConfigFileManagerService configFileManager)
    {
        ImportCommand = new AsyncRelayCommand(Import);
        ExportCommand = new AsyncRelayCommand(Export);
        SearchCommand = new AsyncRelayCommand(Search);
        UpdateViewCommand = new RelayCommand(SendMessage);

        _tagDataFileManager = tagDataFileManager;
        _exportService = exportService;
        _configFileManagerService = configFileManager;

        _tagDataFile = _tagDataFileManager.ReadTagDataFile(configFileManager.DataFilePath);
        _tagsDataGrid = _tagDataFile.TagList;
        OnActivated();
    }

    public List<TDCTag> TagsDataGrid
    {
        get => _tagsDataGrid;
        set => SetProperty(ref _tagsDataGrid, value);
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
        var filePathList = _tagDataFileManager.GetTagFilesToImport();
        if (filePathList != null)
        {
            InitializeImportMessage(filePathList.Length);

            var tasks = new List<Task>();
            Progress<(string fileName, List<TDCTag>? tagList)> p = new();
            p.ProgressChanged += (_, args) =>
            {
                if (args.tagList != null)
                {
                    TagsDataGrid.AddRange(args.tagList);
                    _numberFilesImported++;
                    ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
                    ImportFile = args.fileName;
                }
            };

            foreach (string filePath in filePathList)
            {
                tasks.Add(_tagDataFileManager.GetTagsAsync(filePath, p));
            }

            await Task.WhenAll(tasks);

            TagsDataGrid = TagsDataGrid.Distinct().ToList();
            UpdateTagDataFile();
        }
    }
    private async Task Search()
    {
        TagsDataGrid = await _tagDataFile.Search(TagToSearch).ConfigureAwait(false);
    }

    private async Task Export()
    {
        await _exportService.Export(TagsDataGrid.ToList()).ConfigureAwait(false);
    }

    private void UpdateTagDataFile()
    {
        _tagDataFile.TagList = TagsDataGrid;
        _tagDataFileManager.WriteTagDataToFile(_tagDataFile, _configFileManagerService.DataFilePath);
    }

    private void InitializeImportMessage(int numberFileToImport)
    {
        TagsDataGrid.Clear();
        _numberFilesImported = 0;
        _totalFilesToImport = numberFileToImport;
        ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
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
            var newTagDataFile = _tagDataFileManager.ReadTagDataFile(m.Value);
            r._tagDataFile = newTagDataFile;
            r.TagsDataGrid = newTagDataFile.TagList;
        });
    }
}