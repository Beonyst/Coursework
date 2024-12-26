// Этот файл содержит реализацию команды, которая может быть использована в паттерне MVVM для связывания действий с пользовательским интерфейсом.

#nullable enable

namespace Wpf.Infrastructure;

// Класс, представляющий команду, реализующую интерфейс ICommandWithRaiseCanExecute.
// Команда инкапсулирует действия, которые могут быть выполнены, а также логику проверки, возможно ли их выполнение.
public class Command : ICommandWithRaiseCanExecute
{
    // Делегат для выполнения действия команды
    private readonly Action<object?> _execute;
    
    // Делегат для проверки, может ли быть выполнено действие команды
    private readonly Func<object?, bool>? _canExecute;

    // Событие, которое уведомляет об изменении состояния команды (например, может ли она быть выполнена)
    public event EventHandler? CanExecuteChanged = delegate { };

    // Конструктор команды, принимающий делегат для выполнения действия и необязательный делегат для проверки возможности выполнения
    public Command(Action<object?> execute, Func<object?, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    // Выполнение действия команды
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    // Проверка, может ли команда быть выполнена
    public bool CanExecute(object? parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    // Метод для вызова события изменения возможности выполнения команды
    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged is not null)
        {
            CanExecuteChanged(this, null!);
        }
    }
}
