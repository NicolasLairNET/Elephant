using Elephant.Messages;
using Elephant.Model;
using Elephant_Services.ApplicationConfiguration;
using Elephant_Services.Export;
using Elephant_Services.TagDataFile;
using MessageBox_wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Elephant_wpf.ViewModel;

public class TdcTagViewModel : ObservableRecipient, IViewModel
{
    private readonly ITagDataFileService _tagDataFileService;
    private readonly IExportService _exportService;
    private readonly IConfigFileService _configFileService;

    private List<Tag> _tagsDataGrid;
    private int _numberFilesImported = 0;
    private int _totalFilesToImport = 0;
    private string _tagToSearch = "";
    private string _importMessage = "";
    private string _importFile = "";
    private TagsFile _tagDataFile;

    public IAsyncRelayCommand ImportCommand { get; }
    public IAsyncRelayCommand ExportCommand { get; }
    public IAsyncRelayCommand SearchCommand { get; }
    public IRelayCommand UpdateViewCommand { get; }

    public TdcTagViewModel(
        IExportService exportService,
        ITagDataFileService tagDataFileService,
        IConfigFileService configFileService)
    {
        ImportCommand = new AsyncRelayCommand(Import);
        ExportCommand = new AsyncRelayCommand(Export);
        SearchCommand = new AsyncRelayCommand(Search);
        UpdateViewCommand = new RelayCommand(SendMessage);

        this._tagDataFileService = tagDataFileService;
        this._exportService = exportService;
        this._configFileService = configFileService;

        _tagDataFile = this._tagDataFileService.ReadTagsFile(configFileService.DataFilePath);
        _tagsDataGrid = _tagDataFile.Tags;
        OnActivated();
    }

    public List<Tag> TagsDataGrid
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

    public async Task Import()
    {
        var filePathList = GetTagFilesToImport();
        List<FileImportStatus> messageBoxDetail = new();
        if (filePathList.Length > 0)
        {
            InitializeImportMessage(filePathList.Length);

            var tasks = new List<Task>();
            Progress<(string fileName, List<Tag>? tagList)> importProgress = new();

            importProgress.ProgressChanged += (_, args) =>
            {
                var importDetail = new FileImportStatus();

                if (args.tagList != null)
                {
                    TagsDataGrid.AddRange(args.tagList);
                    _numberFilesImported++;
                    ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
                    ImportFile = args.fileName;

                    importDetail.Name = args.fileName;
                    importDetail.Status = MessageBox_wpf.StatusMessage.Success;
                }
                else
                {
                    _totalFilesToImport--;
                    importDetail.Name = args.fileName;
                    importDetail.Status = MessageBox_wpf.StatusMessage.Error;
                }

                messageBoxDetail.Add(importDetail);
            };

            foreach (string filePath in filePathList)
            {
                tasks.Add(_tagDataFileService.GetTagsAsync(filePath, importProgress!));
            }

            await Task.WhenAll(tasks);

            TagsDataGrid = TagsDataGrid.Distinct().ToList();
            UpdateTagDataFile();
            DisplayImportSummary(messageBoxDetail, _totalFilesToImport);
        }
    }
    public async Task Search()
    {
        TagsDataGrid = await _tagDataFile.Search(TagToSearch).ConfigureAwait(false);
    }

    public async Task Export()
    {
        string? pathDestination = SelectPathExport();
        if (!string.IsNullOrEmpty(pathDestination))
        {
            await _exportService.Export(TagsDataGrid.ToList(), pathDestination).ConfigureAwait(false);
        }
    }

    private void DisplayImportSummary(List<FileImportStatus> importStatus, int filesImported)
    {
        MessageBox_wpf.CustomMessageBox.Show(
        "Résumé de l'import",
        $"{filesImported} fichiers ont été importés",
        MessageBoxButton.OK,
        importStatus
        );
    }

    private void UpdateTagDataFile()
    {
        _tagDataFile.Tags = TagsDataGrid;
        _tagDataFileService.WriteTagsToFile(_tagDataFile, _configFileService.DataFilePath);
    }

    private void InitializeImportMessage(int numberFileToImport)
    {
        TagsDataGrid.Clear();
        _numberFilesImported = 0;
        _totalFilesToImport = numberFileToImport;
        ImportMessage = $"Import en cours {_numberFilesImported} / {_totalFilesToImport} fichiers";
    }

    private string[] GetTagFilesToImport()
    {
        var pathList = Array.Empty<string>();
        OpenFileDialog openFileDialog = new()
        {
            RestoreDirectory = true,
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            pathList = openFileDialog.FileNames;
        }

        return pathList;
    }

    private string? SelectPathExport()
    {
        var defaultFileName = $"export{DateTime.Now:ddMMyyyyHmmss}.csv";
        var defaultPath = _configFileService.ExportFilePath;

        SaveFileDialog saveFileDialog = new();
        saveFileDialog.FileName = defaultFileName;
        saveFileDialog.DefaultExt = ".csv";
        saveFileDialog.InitialDirectory = defaultPath;

        return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : null;
    }

    /// <summary>
    /// Activates the listening of the message which notifies the change of the data file.
    /// When the file is changed the list of tags is updated.
    /// </summary>
    protected override void OnActivated()
    {
        Messenger.Register<TdcTagViewModel, DataFileChangedMessage>(this, (r, m) =>
        {
            var newTagDataFile = _tagDataFileService.ReadTagsFile(m.Value);
            r._tagDataFile = newTagDataFile;
            r.TagsDataGrid = newTagDataFile.Tags;
        });
    }

    public void SendMessage()
    {
        Messenger.Send(new ViewModelChangedMessage("ParameterViewModel"));
    }
}