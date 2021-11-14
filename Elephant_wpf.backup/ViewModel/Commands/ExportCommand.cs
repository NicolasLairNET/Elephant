using Elephant.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Elephant.ViewModel.Commands
{
    internal class ExportCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public ExportCommand(Action execute)
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
