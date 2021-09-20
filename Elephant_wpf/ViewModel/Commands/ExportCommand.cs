using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Elephant.ViewModel.Commands
{
    internal class ExportCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<DataGrid> _execute;

        public ExportCommand(Action<DataGrid> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter as DataGrid);
        }
    }
}
