using Elephant.Messages;
using Elephant.Model;
using Elephant_Services.ApplicationConfiguration;
using Elephant_Services.Export;
using Elephant_Services.TagDataFile;
using Elephant_Services.TagDataFile.FileType;
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
    private readonly ITagDataFileService tagDataFileService;
    private readonly IExportService exportService;
    private readonly IConfigFileService configFileService;

    private List<Tag> tagsDataGrid;
    private int numberFilesImported = 0;
    private int totalFilesToImport = 0;
    private string tagToSearch = "";
    private string importMessage = "";
    private string importFile = "";
    private TagsFile tagDataFile;

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

        this.tagDataFileService = tagDataFileService;
        this.exportService = exportService;
        this.configFileService = configFileService;

        tagDataFile = this.tagDataFileService.ReadTagsFile(configFileService.DataFilePath);
        tagsDataGrid = tagDataFile.Tags;
        OnActivated();
    }

    public List<Tag> TagsDataGrid
    {
        get => tagsDataGrid;
        set => SetProperty(ref tagsDataGrid, value);
    }

    public string TagToSearch
    {
        get => tagToSearch;
        set
        {
            tagToSearch = value;
            SearchCommand.Execute(null);
        }
    }

    public string ImportMessage
    {
        get => importMessage;
        set
        {
            SetProperty<string>(ref importMessage, value);
        }
    }

    public string ImportFile
    {
        get => importFile;
        set
        {
            SetProperty(ref importFile, value);
        }
    }

    public async Task Import()
    {
        var filePathList = GetTagFilesToImport();
        List<FileImportStatus> messageBoxDetail = new();
        if (filePathList.Length > 0)
        {
            ClearDataChoice();
            InitializeImportMessage(filePathList.Length);

            var tasks = new List<Task>();
            Progress<ITDCFile> importProgress = new();

            importProgress.ProgressChanged += (_, tdcFile) =>
            {
                var importDetail = new FileImportStatus();

                if (tdcFile.Tags.Count > 0)
                {
                    TagsDataGrid.AddRange(tdcFile.Tags);
                    numberFilesImported++;
                    ImportMessage = $"Import en cours {numberFilesImported} / {totalFilesToImport} fichiers";
                    ImportFile = tdcFile.FileName;

                    importDetail.Name = tdcFile.FileName;
                    importDetail.Status = StatusMessage.Success;
                }
                else
                {
                    totalFilesToImport--;
                    importDetail.Name = tdcFile.FileName;
                    importDetail.Status = StatusMessage.Error;
                }

                messageBoxDetail.Add(importDetail);
            };

            foreach (string filePath in filePathList)
            {
                tasks.Add(tagDataFileService.GetTagsAsync(filePath, importProgress!));
            }

            await Task.WhenAll(tasks);

            TagsDataGrid = TagsDataGrid.Distinct().ToList();
            UpdateTagDataFile();
            DisplayImportSummary(messageBoxDetail, totalFilesToImport);
        }
    }
    public async Task Search()
    {
        TagsDataGrid = await tagDataFile.Search(TagToSearch).ConfigureAwait(false);
    }

    public async Task Export()
    {
        string? pathDestination = SelectPathExport();
        if (!string.IsNullOrEmpty(pathDestination))
        {
            await exportService.Export(TagsDataGrid.ToList(), pathDestination).ConfigureAwait(false);
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

    /// <summary>
    /// Open a messageBox for ask to user if he wants delete data with a new import.
    /// If the user respond yes Datagrid is cleared.
    /// </summary>
    private void ClearDataChoice()
    {
        if (TagsDataGrid.Count > 0)
        {
            var userChoice = MessageBox_wpf.CustomMessageBox.Show(
                "Mise à jour des données",
                "Nouvel import, voulez-vous supprimer les données existantes ?\nSi vous cliquez sur Non les données seront ajoutées à celles déjà existantes",
                MessageBoxButton.YesNo);

            if (userChoice == MessageBoxResult.Yes)
            {
                TagsDataGrid.Clear();
            }
        }
    }

    private void UpdateTagDataFile()
    {
        tagDataFile.Tags = TagsDataGrid;
        tagDataFileService.WriteTagsToFile(tagDataFile, configFileService.DataFilePath);
    }

    private void InitializeImportMessage(int numberFileToImport)
    {
        numberFilesImported = 0;
        totalFilesToImport = numberFileToImport;
        ImportMessage = $"Import en cours {numberFilesImported} / {totalFilesToImport} fichiers";
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
        var defaultPath = configFileService.ExportFilePath;

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
            var newTagDataFile = tagDataFileService.ReadTagsFile(m.Value);
            r.tagDataFile = newTagDataFile;
            r.TagsDataGrid = newTagDataFile.Tags;
        });
    }

    public void SendMessage()
    {
        Messenger.Send(new ViewModelChangedMessage("ParameterViewModel"));
    }
}