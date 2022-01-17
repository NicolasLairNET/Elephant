using Microsoft.Toolkit.Mvvm.Input;

namespace Elephant.ViewModel;

public interface IViewModel
{
    IRelayCommand UpdateViewCommand { get; }
    public void SendMessage();
}
