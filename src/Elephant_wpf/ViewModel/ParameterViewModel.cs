using Elephant.Messages;
using Elephant_Services.ApplicationConfiguration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Forms;

namespace Elephant.ViewModel;

public class ParameterViewModel : ObservableRecipient, IViewModel
{
    public IRelayCommand UpdateViewCommand { get; }
    public IRelayCommand SelectedDataFileCommand { get; }
    public IRelayCommand SelectedExportFileCommand { get; }
    public IConfigFileService ConfigService { get; }
    private string? _dataFilePath;
    private string? _exportFilePath;

    public ParameterViewModel(IConfigFileService config)
    {
        UpdateViewCommand = new RelayCommand(SendMessage);
        SelectedDataFileCommand = new RelayCommand(SelectDataFile);
        SelectedExportFileCommand = new RelayCommand(SelectExportFolder);
        ConfigService = config;
    }

    /// <summary>
    /// Send to MainViewModel the view to display
    /// </summary>
    public void SendMessage()
    {
        Messenger.Send(new ViewModelChangedMessage("TdcViewModel"));
    }

    public string DataFilePath
    {
        get => ConfigService.DataFilePath;
        set
        {
            if (ConfigService.UpdateDataFile(value))
            {
                // Sending TDCTagViewModel that the data file has been changed
                Messenger.Send(new DataFileChangedMessage(value));
                SetProperty(ref _dataFilePath, value);
            }
        }
    }

    public string ExportFilePath
    {
        get => ConfigService.ExportFilePath;
        set
        {
            ConfigService.UpdateExportFilePath(value);
            SetProperty(ref _exportFilePath, value);
        }
    }

    public void SelectDataFile()
    {
        OpenFileDialog FileDialog = new() {
            DefaultExt = ".json",
            AddExtension = true,
            CheckFileExists = false,
            Filter = "Fichier json (*.json)|*.json"
        };
        FileDialog.ShowDialog();
        if (FileDialog.FileName != "")
        {
            DataFilePath = FileDialog.FileName;
        }
    }

    public void SelectExportFolder()
    {
        FolderBrowserDialog FolderBrowserDialog = new FolderBrowserDialog();
        FolderBrowserDialog.ShowDialog();
        ExportFilePath = FolderBrowserDialog.SelectedPath;
    }
}
