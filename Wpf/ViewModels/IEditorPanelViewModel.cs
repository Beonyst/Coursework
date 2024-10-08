#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

public interface IEditorPanelViewModel
{
    public bool HasErrors { get; }

    public string? ErrorMessage { get; }

    public Command SaveCmd { get; }

    public Command CloseEditorPanelCmd { get; }
}
