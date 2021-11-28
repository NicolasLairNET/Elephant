using System.Windows.Input;

namespace Elephant.ViewModel;

public class Command : ICommand
{
    readonly Action _targetExecuteMethod;
    Func<bool> _targetCanExecuteMethod;
    public Command(Action executeMethod) => _targetExecuteMethod = executeMethod;

    public Command(Action executeMethod, Func<bool> canExecuteMethod)
    {
        _targetExecuteMethod = executeMethod;
        _targetCanExecuteMethod = canExecuteMethod;
    }

    public event EventHandler CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged is not null)
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    public bool CanExecute(object parameter)
    {
        if (_targetCanExecuteMethod is not null)
        {
            return _targetCanExecuteMethod();
        }

        if (_targetExecuteMethod is not null)
        {
            return true;
        }

        return false;
    }

    public void Execute(object parameter)
    {
        if (_targetExecuteMethod is not null)
        {
            _targetExecuteMethod();
        }
    }
}
