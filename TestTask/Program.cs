#nullable enable
using API;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров для обработки HTTP запросов.
builder.Services.AddControllers();

// Добавление сервисов для работы с API (для документации Swagger).
builder.Services.AddEndpointsApiExplorer();  // Для автоматического документирования API.
builder.Services.AddSwaggerGen();  // Генерация Swagger UI.

var startup = new Startup(builder.Environment, builder.Configuration);
startup.ConfigureServices(builder.Services);  // Настройка сервисов через метод Startup.ConfigureServices.

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());  // Настройка контейнера зависимостей через Autofac.
builder.Host.ConfigureContainer<ContainerBuilder>(startup.ConfigureContainer);  // Конфигурация контейнера с помощью модулей.

var app = builder.Build();
startup.Configure(app, app.Environment);  // Настройка middleware через метод Startup.Configure.

// Настройка конвейера обработки HTTP запросов.
// Условие для включения Swagger UI в режиме разработки.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Включение Swagger для документации.
    app.UseSwaggerUI();  // Включение UI для Swagger.
}*/

app.Run();  // Запуск приложения.
