using Microsoft.Toolkit.Mvvm.Input;

namespace Elephant_wpf.ViewModel;

public interface IViewModel
{
    IRelayCommand UpdateViewCommand { get; }
    public void SendMessage();
}
