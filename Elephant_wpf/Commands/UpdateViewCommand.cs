using Elephant.ViewModel;
using Elephant_wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Elephant.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel _viewModel;
        private TdcTagViewModel _tdcTagViewModel;
        private ParameterViewModel _parameterViewModel;

        public UpdateViewCommand(MainViewModel viewModel, TdcTagViewModel tdcTagViewModel, ParameterViewModel parameterViewModel)
        {
            _viewModel = viewModel;
            _tdcTagViewModel = tdcTagViewModel;
            _parameterViewModel = parameterViewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter.ToString() == "Home")
            {
                _viewModel.SelectedViewModel = _tdcTagViewModel;
            }
            else if (parameter.ToString() == "Parameter")
            {
                _viewModel.SelectedViewModel = _parameterViewModel;
            }
        }
    }
}
