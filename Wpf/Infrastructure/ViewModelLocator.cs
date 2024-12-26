// Класс ViewModelLocator отвечает за разрешение зависимостей для ViewModel с использованием контейнера Autofac.

#nullable enable
using Autofac;
using Autofac.Core;

namespace Wpf.Infrastructure;

// Класс, который используется для поиска и разрешения ViewModel через контейнер зависимостей.
public class ViewModelLocator
{
    private readonly ILifetimeScope _container; // Контейнер для разрешения зависимостей

    // Конструктор, принимающий контейнер зависимостей.
    public ViewModelLocator(ILifetimeScope container)
    {
        _container = container; // Инициализация контейнера
    }

    // Метод для получения экземпляра ViewModel с возможностью передачи параметров.
    public TViewModel Get<TViewModel>(params Parameter[] parameters) where TViewModel : IViewModel
    {
        // Разрешение зависимости TViewModel через контейнер с параметрами.
        var viewModel = _container.Resolve<TViewModel>(parameters);

        return viewModel; // Возврат разрешенного экземпляра ViewModel
    }
}
