// Абстрактный класс ViewModelBase реализует базовую функциональность для всех ViewModel, включая уведомления об изменении свойств и работу с командами.

#nullable enable
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.Infrastructure;

// Абстрактный класс, реализующий интерфейс IViewModel. Все ViewModel должны наследовать от этого класса.
public abstract class ViewModelBase : IViewModel
{
    // Диспетчер для работы с потоками и UI в WPF приложении.
    protected Dispatcher CurrentDispatcher = Application.Current.Dispatcher;

    // Событие, которое срабатывает при изменении значения свойства.
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    // Метод для поднятия события PropertyChanged, чтобы оповестить об изменении свойства.
    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Метод для уведомления команды о том, что нужно обновить ее состояние (например, доступность).
    protected virtual void RaiseCommandCanExecuteChanged(ICommandWithRaiseCanExecute command)
    {
        command.RaiseCanExecuteChanged();
    }
}
