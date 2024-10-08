#nullable enable
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Wpf.Infrastructure;
using Wpf.Models.Settings;
using Wpf.Services.Implementations;
using Wpf.Services.Interfaces;
using Wpf.ViewModels;
using Wpf.ViewModels.MedicinesEditor;
using Wpf.ViewModels.SuppliersEditor;

namespace Wpf;

public class BootstrapperAutofac
{
    private const string _windowTitle = "Управление аптекой";

    public IContainer Container { get; } = null!;

    public NavigationViewModel MainWindowViewModel
    {
        get => Container.Resolve<NavigationViewModel>(
            new NamedParameter("initialViewModel", Container.Resolve<MainViewModel>()), new NamedParameter("windowTitle", _windowTitle));
    }

    public BootstrapperAutofac(IConfiguration configuration)
    {
        AppSettings appSettings = configuration.Get<AppSettings>()!;

        var builder = new ContainerBuilder();

        builder.Register(_ => new ApiSettings { BaseUrl = appSettings!.ApiBaseAddress })
        .As<ApiSettings>()
        .InstancePerLifetimeScope();

        builder.Register(_ => appSettings)
            .As<AppSettings>()
            .SingleInstance();

        builder.RegisterType<ApiHttpClientFactory>()
            .As<IApiHttpClientFactory>()
            .SingleInstance();
        builder.RegisterType<ViewModelLocator>()
            .SingleInstance();
        builder.RegisterType<NavigationManager>()
            .SingleInstance();
        builder.RegisterType<NavigationViewModel>()
            .SingleInstance();
        builder.RegisterType<MainViewModel>();
        builder.RegisterType<AddSupplierViewModel>();
        builder.RegisterType<DeleteSupplierViewModel>();
        builder.RegisterType<EditSupplierViewModel>();
        builder.RegisterType<SuppliersEditorViewModel>();
        builder.RegisterType<AddMedicineViewModel>();
        builder.RegisterType<DeleteMedicineViewModel>();
        builder.RegisterType<EditMedicineViewModel>();
        builder.RegisterType<MedicinesEditorViewModel>();

        RegisterServiceWithHttpClient<SupplierService, ISupplierService>(builder);
        RegisterServiceWithHttpClient<MedicineService, IMedicineService>(builder);

        Container = builder.Build();
    }

    private void RegisterServiceWithHttpClient<TImplementation, TInterface>(ContainerBuilder builder) where TInterface : notnull where TImplementation : notnull
    {
        builder.RegisterType<TImplementation>()
            .As<TInterface>()
            .WithParameter(new ResolvedParameter((pi, _) => pi.ParameterType == typeof(HttpClient),
            (_, context) => context.Resolve<IApiHttpClientFactory>().GetUnauthorizedClient()))
            .InstancePerLifetimeScope();
    }
}
