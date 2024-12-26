#nullable enable
using API.Data.Implementations;
using API.Modules;
using API.Settings;
using Autofac;

namespace API;

// Класс Startup настраивает приложение, конфигурирует сервисы, контейнер зависимостей и middleware.
public class Startup
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public IConfiguration Configuration { get; }

    // Конструктор принимает параметры окружения и конфигурации для настройки приложения.
    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        _hostingEnvironment = env;
        Configuration = configuration;
    }

    // Метод для регистрации сервисов в DI контейнере (сервисах ASP.NET).
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration);  // Регистрация настроек приложения из конфигурации.
        services.AddDbContext<PharmacyDbContext>();  // Регистрация контекста базы данных.
        services.AddHttpContextAccessor();  // Добавление HttpContextAccessor для доступа к текущему HTTP контексту.

        services.AddControllers();  // Регистрация контроллеров MVC.
    }

    // Метод для регистрации модулей в контейнере зависимостей Autofac.
    public void ConfigureContainer(HostBuilderContext hostBuilderContext, ContainerBuilder builder)
    {
        builder.RegisterModule(new RepositoriesModule());  // Регистрация модуля репозиториев.
        builder.RegisterModule(new ServicesModule());  // Регистрация модуля сервисов.
    }

    // Метод для настройки middleware в конвейере обработки запросов.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();  // Включение маршрутизации для обработки HTTP запросов.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();  // Настройка маршрута по умолчанию для контроллеров.
        });
    }
}
