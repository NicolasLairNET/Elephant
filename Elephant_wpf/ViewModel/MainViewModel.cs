using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Elephant.ViewModel;
using Elephant.Commands;

namespace Elephant.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetProperty(ref _selectedViewModel, value);
        }

        public MainViewModel(TdcTagViewModel tdcTagViewModel, ParameterViewModel parameterViewModel)
        {
            _selectedViewModel = tdcTagViewModel;
            UpdateViewCommand = new UpdateViewCommand(this, tdcTagViewModel, parameterViewModel);
        }

        public ICommand UpdateViewCommand { get; set;}
    }
}
