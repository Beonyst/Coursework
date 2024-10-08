#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

public abstract class EditorViewModelBase : ViewModelBase
{
    public string Name { get; } = null!;

    public MainViewModel ParentViewModel { get; set; } = null!;

    protected EditorViewModelBase(string name)
    {
        Name = name;
    }

    public abstract void Update();
}
