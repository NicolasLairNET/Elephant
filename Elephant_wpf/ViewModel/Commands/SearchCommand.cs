using System.Windows.Input;

namespace Elephant.ViewModel.Commands;

internal class SearchCommand : ICommand
{
    public event EventHandler CanExecuteChanged;
    private Action<string> _execute;

    public SearchCommand(Action<string> execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _execute.Invoke(parameter as string);
    }
}

