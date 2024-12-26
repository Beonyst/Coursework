// Интерфейс ICloseConfirmingViewModel предоставляет метод для подтверждения закрытия окна, который может быть реализован в ViewModel,
// если необходимо запросить подтверждение у пользователя перед закрытием.

#nullable enable

namespace Wpf.Infrastructure;

// Интерфейс для ViewModel, которые требуют подтверждения перед закрытием окна.
public interface ICloseConfirmingViewModel
{
    // Метод для подтверждения закрытия окна.
    bool ConfirmWindowClose();
}
