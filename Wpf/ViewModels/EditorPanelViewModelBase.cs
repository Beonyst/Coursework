#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

// Абстрактный класс EditorPanelViewModelBase предоставляет общую функциональность для работы с панелью редактора, 
// включая обработку ошибок и команды для сохранения и закрытия панели.
public abstract class EditorPanelViewModelBase<TEditedModel> : ViewModelBase, IEditorPanelViewModel
    where TEditedModel : class
{
    private bool _hasErrors;
    private string? _errorMessage;

    private Command _saveCmd = null!;
    private Command _closeEditorPanelCmd = null!;

    // Свойство, указывающее, есть ли ошибки в текущей панели редактора
    public bool HasErrors
    {
        get => _hasErrors;
        set
        {
            if (_hasErrors != value) // Проверка на изменение состояния ошибок
            {
                _hasErrors = value;
                RaisePropertyChanged(nameof(HasErrors)); // Уведомление об изменении свойства
            }
        }
    }

    // Свойство для получения или установки сообщения об ошибке
    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value) // Проверка на изменение сообщения
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage)); // Уведомление об изменении сообщения
            }
        }
    }

    // Событие, которое срабатывает при закрытии панели редактора
    public event EventHandler<EditorPanelClosedEventArgs<TEditedModel>> EditorPanelClosed = delegate { };

    // Команда для сохранения данных
    public Command SaveCmd
    {
        get => _saveCmd ??= new Command(Save); // Инициализация команды сохранения
    }

    // Команда для закрытия панели редактора
    public Command CloseEditorPanelCmd
    {
        get => _closeEditorPanelCmd ??= new Command(_ =>
        {
            ClosePanel(EditorPanelResult.Canceled, null); // Закрытие панели с результатом отмены
            ErrorMessage = null; // Очистка сообщения об ошибке
            HasErrors = false; // Сброс состояния ошибок
        });
    }

    // Метод для закрытия панели с передачей результата и редактируемой модели
    protected virtual void ClosePanel(EditorPanelResult editorPanelResult, TEditedModel? editedModel)
    {
        EditorPanelClosed(this, new EditorPanelClosedEventArgs<TEditedModel>(editorPanelResult, editedModel));
    }

    // Метод для отображения сообщения об ошибке
    protected void ShowErrorMessage(string errorMessage)
    {
        ErrorMessage = errorMessage;
        HasErrors = true;
    }

    // Метод для сохранения данных (по умолчанию вызывает закрытие панели с результатом отмены)
    protected virtual void Save(object? obj)
    {
        ClosePanel(EditorPanelResult.Canceled, null); // Закрытие панели с результатом отмены
    }
}
