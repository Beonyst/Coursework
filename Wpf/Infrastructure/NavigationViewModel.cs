#nullable enable

namespace Wpf.Infrastructure;

public class NavigationViewModel : ViewModelBase, ICloseConfirmingViewModel
{
    private ViewModelBase _currentViewModel = null!;
    private readonly NavigationManager _navigationManager;
    private bool _isLoadingVisible;
    private string _windowTitle = null!;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }
    }

    public bool IsLoadingVisible
    {
        get => _isLoadingVisible;
        set
        {
            _isLoadingVisible = value;
            RaisePropertyChanged(nameof(IsLoadingVisible));
        }
    }

    public string WindowTitle
    {
        get => _windowTitle;
        set
        {
            _windowTitle = value;
            RaisePropertyChanged(nameof(WindowTitle));
        }
    }

    public NavigationViewModel(ViewModelBase initialViewModel, NavigationManager navigationManager, string windowTitle)
    {
        CurrentViewModel = initialViewModel;
        _windowTitle = windowTitle;
        _navigationManager = navigationManager;
        _navigationManager.NavigationChanged += NavigationManagerOnNavigationChanged;
        _navigationManager.LoadingViewRequested += NavigationManagerOnLoadingViewRequested;
        _navigationManager.LoadingViewStopped += NavigationManagerOnLoadingViewStopped;
    }

    private void NavigationManagerOnNavigationChanged(ViewModelBase viewModel)
    {
        CurrentViewModel = viewModel;
    }

    private void NavigationManagerOnLoadingViewRequested(object? sender, EventArgs eventArgs)
    {
        IsLoadingVisible = true;
    }

    private void NavigationManagerOnLoadingViewStopped(object? sender, EventArgs eventArgs)
    {
        IsLoadingVisible = false;
    }

    public bool ConfirmWindowClose()
    {
        if (CurrentViewModel is ICloseConfirmingViewModel closeConfirmingViewModel)
        {
            return closeConfirmingViewModel.ConfirmWindowClose();
        }

        return true;
    }
}
