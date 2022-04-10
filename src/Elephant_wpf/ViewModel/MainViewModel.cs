using Elephant.Messages;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant_wpf.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        private IViewModel _selectedViewModel;
        private TdcTagViewModel _tdcTagViewModel;
        private ParameterViewModel _parameterViewModel;

        public IViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetProperty(ref _selectedViewModel, value);
        }

        public MainViewModel(TdcTagViewModel tdcTagViewModel, ParameterViewModel parameterViewModel)
        {
            _selectedViewModel = tdcTagViewModel;
            this._tdcTagViewModel = tdcTagViewModel;
            this._parameterViewModel = parameterViewModel;
            OnActivated();
        }

        protected override void OnActivated()
        {
            Messenger.Register<MainViewModel, ViewModelChangedMessage>(this, (r, m) =>
            {
                r.SelectedViewModel = m.Value switch
                {
                    "TdcViewModel" => _tdcTagViewModel,
                    "ParameterViewModel" => _parameterViewModel,
                    _ => r.SelectedViewModel
                };
            });
        }
    }
}
