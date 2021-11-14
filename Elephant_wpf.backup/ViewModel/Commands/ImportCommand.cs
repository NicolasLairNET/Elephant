using System;
using System.Windows.Input;

namespace Elephant.ViewModel.Commands
{
    public class ImportCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public ImportCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
