// Класс NavigationViewModel управляет навигацией между различными ViewModel и отображением загрузки, а также заголовком окна. 
// Он реализует интерфейс ICloseConfirmingViewModel для подтверждения закрытия окна.

#nullable enable

namespace Wpf.Infrastructure;

// Класс, отвечающий за управление текущей ViewModel, состоянием загрузки и заголовком окна.
// Он также обрабатывает события навигации и отображения/скрытия индикатора загрузки.
public class NavigationViewModel : ViewModelBase, ICloseConfirmingViewModel
{
    private ViewModelBase _currentViewModel = null!; // Текущая ViewModel
    private readonly NavigationManager _navigationManager; // Менеджер навигации
    private bool _isLoadingVisible; // Флаг видимости индикатора загрузки
    private string _windowTitle = null!; // Заголовок окна

    // Свойство для доступа и изменения текущей ViewModel. 
    // При изменении вызывает уведомление об изменении свойства.
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }
    }

    // Свойство для управления видимостью индикатора загрузки.
    // При изменении вызывает уведомление об изменении свойства.
    public bool IsLoadingVisible
    {
        get => _isLoadingVisible;
        set
        {
            _isLoadingVisible = value;
            RaisePropertyChanged(nameof(IsLoadingVisible));
        }
    }

    // Свойство для управления заголовком окна.
    // При изменении вызывает уведомление об изменении свойства.
    public string WindowTitle
    {
        get => _windowTitle;
        set
        {
            _windowTitle = value;
            RaisePropertyChanged(nameof(WindowTitle));
        }
    }

    // Конструктор, инициализирующий начальную ViewModel, менеджер навигации и заголовок окна.
    // Также подписывается на события изменений навигации и загрузки.
    public NavigationViewModel(ViewModelBase initialViewModel, NavigationManager navigationManager, string windowTitle)
    {
        CurrentViewModel = initialViewModel;
        _windowTitle = windowTitle;
        _navigationManager = navigationManager;
        _navigationManager.NavigationChanged += NavigationManagerOnNavigationChanged; // Обработка изменения навигации
        _navigationManager.LoadingViewRequested += NavigationManagerOnLoadingViewRequested; // Обработка запроса отображения индикатора загрузки
        _navigationManager.LoadingViewStopped += NavigationManagerOnLoadingViewStopped; // Обработка остановки индикатора загрузки
    }

    // Обработчик события изменения навигации. Обновляет текущую ViewModel.
    private void NavigationManagerOnNavigationChanged(ViewModelBase viewModel)
    {
        CurrentViewModel = viewModel;
    }

    // Обработчик события запроса на отображение индикатора загрузки. Устанавливает флаг видимости индикатора в true.
    private void NavigationManagerOnLoadingViewRequested(object? sender, EventArgs eventArgs)
    {
        IsLoadingVisible = true;
    }

    // Обработчик события остановки отображения индикатора загрузки. Устанавливает флаг видимости индикатора в false.
    private void NavigationManagerOnLoadingViewStopped(object? sender, EventArgs eventArgs)
    {
        IsLoadingVisible = false;
    }

    // Метод для подтверждения закрытия окна, если текущая ViewModel реализует ICloseConfirmingViewModel.
    public bool ConfirmWindowClose()
    {
        if (CurrentViewModel is ICloseConfirmingViewModel closeConfirmingViewModel)
        {
            return closeConfirmingViewModel.ConfirmWindowClose(); // Передает запрос на подтверждение закрытия текущей ViewModel
        }

        return true; // Если текущая ViewModel не требует подтверждения, возвращает true
    }
}
