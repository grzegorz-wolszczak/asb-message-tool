using System;
using System.Windows.Input;

namespace Main.Commands;

internal class DelegateCommand : ICommand
{
    private Action<object> _execute;
    private Predicate<object> _canExecute;

    public DelegateCommand(Action<object> onExecuteMethod, Predicate<object> onCanExecuteMethod = null)
    {
        _execute = onExecuteMethod;
        _canExecute = onCanExecuteMethod ?? (arg => true);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}