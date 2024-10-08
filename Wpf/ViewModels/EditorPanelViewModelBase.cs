#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

public abstract class EditorPanelViewModelBase<TEditedModel> : ViewModelBase, IEditorPanelViewModel
    where TEditedModel : class
{
    private bool _hasErrors;
    private string? _errorMessage;

    private Command _saveCmd = null!;
    private Command _closeEditorPanelCmd = null!;

    public bool HasErrors
    {
        get => _hasErrors;
        set
        {
            if (_hasErrors != value)
            {
                _hasErrors = value;
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
    }

    public event EventHandler<EditorPanelClosedEventArgs<TEditedModel>> EditorPanelClosed = delegate { };

    public Command SaveCmd
    {
        get => _saveCmd ??= new Command(Save);
    }

    public Command CloseEditorPanelCmd
    {
        get => _closeEditorPanelCmd ??= new Command(_ =>
        {
            ClosePanel(EditorPanelResult.Canceled, null);
            ErrorMessage = null;
            HasErrors = false;
        });
    }

    protected virtual void ClosePanel(EditorPanelResult editorPanelResult, TEditedModel? editedModel)
    {
        EditorPanelClosed(this, new EditorPanelClosedEventArgs<TEditedModel>(editorPanelResult, editedModel));
    }

    protected void ShowErrorMessage(string errorMessage)
    {
        ErrorMessage = errorMessage;
        HasErrors = true;
    }

    protected virtual void Save(object? obj)
    {
        ClosePanel(EditorPanelResult.Canceled, null);
    }
}
