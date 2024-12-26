// Этот файл содержит класс, который используется для передачи информации о закрытии панели редактора.

#nullable enable

namespace Wpf.Infrastructure;

// Класс, представляющий аргументы события, которое срабатывает при закрытии панели редактора.
// Он содержит информацию о результате операции и редактируемой модели.
public class EditorPanelClosedEventArgs<TEditedModel>
{
    // Результат операции (успех или отмена)
    public EditorPanelResult ResultType { get; }

    // Модель, которая была отредактирована
    public TEditedModel EditedModel { get; }

    // Конструктор класса, принимающий результат операции и отредактированную модель
    public EditorPanelClosedEventArgs(EditorPanelResult resultType, TEditedModel editedModel)
    {
        ResultType = resultType;
        EditedModel = editedModel;
    }
}
