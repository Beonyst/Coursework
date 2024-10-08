#nullable enable
using Autofac.Core;

namespace Wpf.Infrastructure;

public delegate void NavigationChangedDelegate(ViewModelBase viewModel);

public class NavigationManager
{
    private readonly ViewModelLocator _viewModelLocator;

    public event NavigationChangedDelegate NavigationChanged = delegate { };
    public event EventHandler LoadingViewRequested = delegate { };
    public event EventHandler LoadingViewStopped = delegate { };

    public NavigationManager(ViewModelLocator viewModelLocator)
    {
        _viewModelLocator = viewModelLocator;
    }

    public async void NavigateTo<TViewModel>(params Parameter[] parameters) where TViewModel : ViewModelBase
    {
        TViewModel? viewModel = null;
        await Loading(() => viewModel = _viewModelLocator.Get<TViewModel>(parameters));
        NavigationChanged(viewModel!);
    }

    private async Task Loading(Action loadingAction)
    {
        LoadingViewRequested(this, null!);

        await Task.Run(loadingAction);

        LoadingViewStopped(this, null!);
    }
}
