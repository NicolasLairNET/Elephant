using Elephant.Messages;
using Elephant_Services.ApplicationConfiguration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.IO;
using System.Windows.Forms;
using MessageBox_wpf;
using System.Windows;

namespace Elephant_wpf.ViewModel;

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
        ConfigService = config;
        UpdateViewCommand = new RelayCommand(SendMessage);
        SelectedDataFileCommand = new RelayCommand(SelectDataFile);
        SelectedExportFileCommand = new RelayCommand(SelectExportFolder);
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
            if (!File.Exists(FileDialog.FileName))
            {
                var response = MessageBox_wpf.CustomMessageBox.Show(
                    "Le fichier n'existe pas voulez-vous le créer ?",
                    MessageBox_wpf.MessageBoxType.ConfirmationWithYesNo);

                if (response != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            DataFilePath = FileDialog.FileName;
        }
    }

    public void SelectExportFolder()
    {
        FolderBrowserDialog fbd = new();
        fbd.ShowDialog();
        ExportFilePath = fbd.SelectedPath;
    }
}
