using Elephant.Messages;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace Elephant.ViewModel
{
    public class ParameterViewModel : ObservableRecipient, IViewModel
    {
        public ICommand UpdateViewCommand { get; }

        public ParameterViewModel()
        {
            UpdateViewCommand = new RelayCommand(SendMessage);
        }

        public void SendMessage()
        {
            Messenger.Send(new ViewModelChangedMessage("TdcViewModel"));
        }
    }
}
