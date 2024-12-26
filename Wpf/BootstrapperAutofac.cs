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

/// <summary>
/// Класс для настройки контейнера зависимостей с использованием Autofac.
/// Отвечает за регистрацию и разрешение зависимостей в приложении.
/// </summary>
public class BootstrapperAutofac
{
    /// <summary>
    /// Заголовок главного окна.
    /// </summary>
    private const string _windowTitle = "Управление аптекой";

    /// <summary>
    /// Экземпляр контейнера Autofac для управления зависимостями.
    /// </summary>
    public IContainer Container { get; } = null!;

    /// <summary>
    /// Получает главный ViewModel приложения, разрешая зависимости.
    /// </summary>
    public NavigationViewModel MainWindowViewModel
    {
        get => Container.Resolve<NavigationViewModel>(
            new NamedParameter("initialViewModel", Container.Resolve<MainViewModel>()),
            new NamedParameter("windowTitle", _windowTitle));
    }

    /// <summary>
    /// Конструктор. Инициализирует контейнер зависимостей и регистрирует все необходимые сервисы и модели.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения.</param>
    public BootstrapperAutofac(IConfiguration configuration)
    {
        // Получение настроек приложения из конфигурации.
        AppSettings appSettings = configuration.Get<AppSettings>()!;

        var builder = new ContainerBuilder();

        // Регистрация настроек API.
        builder.Register(_ => new ApiSettings { BaseUrl = appSettings!.ApiBaseAddress })
               .As<ApiSettings>()
               .InstancePerLifetimeScope();

        // Регистрация общих настроек приложения.
        builder.Register(_ => appSettings)
               .As<AppSettings>()
               .SingleInstance();

        // Регистрация фабрики HTTP-клиентов.
        builder.RegisterType<ApiHttpClientFactory>()
               .As<IApiHttpClientFactory>()
               .SingleInstance();

        // Регистрация вспомогательных классов и ViewModel.
        builder.RegisterType<ViewModelLocator>()
               .SingleInstance();
        builder.RegisterType<NavigationManager>()
               .SingleInstance();
        builder.RegisterType<NavigationViewModel>()
               .SingleInstance();

        // Регистрация основных ViewModel приложения.
        builder.RegisterType<MainViewModel>();
        builder.RegisterType<AddSupplierViewModel>();
        builder.RegisterType<DeleteSupplierViewModel>();
        builder.RegisterType<EditSupplierViewModel>();
        builder.RegisterType<SuppliersEditorViewModel>();
        builder.RegisterType<AddMedicineViewModel>();
        builder.RegisterType<DeleteMedicineViewModel>();
        builder.RegisterType<EditMedicineViewModel>();
        builder.RegisterType<MedicinesEditorViewModel>();

        // Регистрация сервисов с использованием HTTP-клиентов.
        RegisterServiceWithHttpClient<SupplierService, ISupplierService>(builder);
        RegisterServiceWithHttpClient<MedicineService, IMedicineService>(builder);

        // Построение контейнера зависимостей.
        Container = builder.Build();
    }

    /// <summary>
    /// Метод для регистрации сервисов, работающих с HTTP-клиентами.
    /// </summary>
    /// <typeparam name="TImplementation">Класс-реализация сервиса.</typeparam>
    /// <typeparam name="TInterface">Интерфейс сервиса.</typeparam>
    /// <param name="builder">Строитель контейнера.</param>
    private void RegisterServiceWithHttpClient<TImplementation, TInterface>(ContainerBuilder builder)
        where TInterface : notnull 
        where TImplementation : notnull
    {
        builder.RegisterType<TImplementation>()
               .As<TInterface>()
               .WithParameter(new ResolvedParameter(
                   (pi, _) => pi.ParameterType == typeof(HttpClient),
                   (_, context) => context.Resolve<IApiHttpClientFactory>().GetUnauthorizedClient()))
               .InstancePerLifetimeScope();
    }
}
