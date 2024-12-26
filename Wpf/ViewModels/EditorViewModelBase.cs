#nullable enable
using Wpf.Infrastructure;

namespace Wpf.ViewModels;

// Абстрактный класс EditorViewModelBase служит базой для всех редакторов и определяет общие свойства и методы.
public abstract class EditorViewModelBase : ViewModelBase
{
    // Свойство для хранения имени редактора
    public string Name { get; } = null!;

    // Свойство для хранения родительского ViewModel
    public MainViewModel ParentViewModel { get; set; } = null!;

    // Конструктор, который инициализирует имя редактора
    protected EditorViewModelBase(string name)
    {
        Name = name;
    }

    // Абстрактный метод, который должен быть реализован в дочерних классах для обновления состояния редактора
    public abstract void Update();
}
