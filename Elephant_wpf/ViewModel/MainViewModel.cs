using Elephant.Messages;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        private IViewModel selectedViewModel;
        private TdcTagViewModel tdcTagViewModel;
        private ParameterViewModel parameterViewModel;

        public IViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set => SetProperty(ref selectedViewModel, value);
        }

        public MainViewModel(TdcTagViewModel tdcTagViewModel, ParameterViewModel parameterViewModel)
        {
            selectedViewModel = tdcTagViewModel;
            this.tdcTagViewModel = tdcTagViewModel;
            this.parameterViewModel = parameterViewModel;
            OnActivated();
        }

        protected override void OnActivated()
        {
            Messenger.Register<MainViewModel, ViewModelChangedMessage>(this, (r, m) =>
            {
                if (m.Value == "TdcViewModel")
                {
                    r.SelectedViewModel = tdcTagViewModel;
                }
                else if (m.Value == "ParameterViewModel")
                {
                    r.SelectedViewModel = parameterViewModel;
                }
            });
        }
    }
}
