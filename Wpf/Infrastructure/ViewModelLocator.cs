#nullable enable
using Autofac;
using Autofac.Core;

namespace Wpf.Infrastructure;

public class ViewModelLocator
{
    private readonly ILifetimeScope _container;

    public ViewModelLocator(ILifetimeScope container)
    {
        _container = container;
    }

    public TViewModel Get<TViewModel>(params Parameter[] parameters) where TViewModel : IViewModel
    {
        var viewModel = _container.Resolve<TViewModel>(parameters);

        return viewModel;
    }
}
