// Класс NavigationManager управляет навигацией между различными ViewModel, а также отображением индикатора загрузки.
// Он использует делегат для уведомления о изменениях навигации и отображении индикатора загрузки.

#nullable enable
using Autofac.Core;

namespace Wpf.Infrastructure;

// Делегат, который используется для события изменения навигации.
public delegate void NavigationChangedDelegate(ViewModelBase viewModel);

public class NavigationManager
{
    private readonly ViewModelLocator _viewModelLocator; // Локатор ViewModel для получения экземпляров ViewModel

    // Событие, которое генерируется при изменении текущей ViewModel.
    public event NavigationChangedDelegate NavigationChanged = delegate { };

    // Событие, которое генерируется при запросе на отображение индикатора загрузки.
    public event EventHandler LoadingViewRequested = delegate { };

    // Событие, которое генерируется при остановке отображения индикатора загрузки.
    public event EventHandler LoadingViewStopped = delegate { };

    // Конструктор, который инициализирует локатор ViewModel.
    public NavigationManager(ViewModelLocator viewModelLocator)
    {
        _viewModelLocator = viewModelLocator; // Инициализация локатора ViewModel
    }

    // Метод для навигации к новой ViewModel. Выполняет асинхронную загрузку и вызывает событие изменения навигации.
    public async void NavigateTo<TViewModel>(params Parameter[] parameters) where TViewModel : ViewModelBase
    {
        TViewModel? viewModel = null;

        // Выполняет асинхронную загрузку новой ViewModel с отображением индикатора загрузки.
        await Loading(() => viewModel = _viewModelLocator.Get<TViewModel>(parameters));

        // Генерация события изменения текущей ViewModel.
        NavigationChanged(viewModel!);
    }

    // Метод для отображения индикатора загрузки и выполнения асинхронного действия.
    private async Task Loading(Action loadingAction)
    {
        // Генерация события запроса на отображение индикатора загрузки.
        LoadingViewRequested(this, null!);

        // Выполнение асинхронного действия в отдельном потоке.
        await Task.Run(loadingAction);

        // Генерация события остановки индикатора загрузки.
        LoadingViewStopped(this, null!);
    }
}
