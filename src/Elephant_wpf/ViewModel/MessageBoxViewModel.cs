using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Elephant_wpf.ViewModel
{
    public class MessageBoxViewModel : ObservableRecipient
    {
        private string _message;

        public MessageBoxViewModel()
        {
            _message = "Aucun message";
        }

        public string Message
        {
            get { return _message; }
            set => SetProperty(ref _message, value);
        }
    }
}
