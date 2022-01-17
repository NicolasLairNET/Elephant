using Elephant.Messages;
using Elephant.Services.ConfigFileManagerService;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant.ViewModel
{
    public class ParameterViewModel : ObservableRecipient, IViewModel
    {
        public IRelayCommand UpdateViewCommand { get; }
        public IConfigFileManagerService ConfigService { get; }
        private string? _dataFilePath;
        private string? _exportFilePath;

        public ParameterViewModel(IConfigFileManagerService config)
        {
            UpdateViewCommand = new RelayCommand(SendMessage);
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
                SetProperty(ref _dataFilePath, value);
                ConfigService.UpdateDataFile(value);
                // Sending TDCTagViewModel that the data file has been changed
                Messenger.Send(new DataFileChangedMessage(value));
            }
        }

        public string ExportFilePath
        {
            get => ConfigService.ExportFilePath;
            set
            {
                SetProperty(ref _exportFilePath, _exportFilePath);
                ConfigService.UpdateExportFile(value);
            }
        }
    }
}
