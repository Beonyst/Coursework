#nullable enable

namespace Wpf.Infrastructure;

public class Command : ICommandWithRaiseCanExecute
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged = delegate { };

    public Command(Action<object?> execute, Func<object?, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged is not null)
        {
            CanExecuteChanged(this, null!);
        }
    }
}
