#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

// Интерфейс IEditorPanelViewModel определяет основные свойства и команды для работы с панелью редактора.
public interface IEditorPanelViewModel
{
    // Свойство, которое указывает, есть ли ошибки в панели редактора
    public bool HasErrors { get; }

    // Свойство, которое возвращает сообщение об ошибке, если оно есть
    public string? ErrorMessage { get; }

    // Команда для сохранения изменений в панели редактора
    public Command SaveCmd { get; }

    // Команда для закрытия панели редактора
    public Command CloseEditorPanelCmd { get; }
}
