// Интерфейс ICommandWithRaiseCanExecute расширяет ICommand, добавляя метод для принудительного обновления состояния команды 
// (например, доступности команды), что позволяет вручную вызывать изменение состояния выполнения команды.

#nullable enable
using System.Windows.Input;

namespace Wpf.Infrastructure;

// Интерфейс, который расширяет ICommand, добавляя метод для принудительного обновления состояния команды.
public interface ICommandWithRaiseCanExecute : ICommand
{
    // Метод для принудительного обновления состояния команды (например, доступности).
    void RaiseCanExecuteChanged();
}
