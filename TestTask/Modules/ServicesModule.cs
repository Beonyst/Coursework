#nullable enable
using API.Data.Services.Implementations;
using Autofac;
using Module = Autofac.Module;

namespace API.Modules;

// Модуль, который регистрирует сервисы в контейнере зависимостей Autofac.
public class ServicesModule : Module
{
    // Переопределённый метод для регистрации сервисов в контейнере.
    protected override void Load(ContainerBuilder builder)
    {
        // Получение сборки сервисов для динамической регистрации всех классов, заканчивающихся на "Service".
        var servicesAssembly = typeof(MedicineService).Assembly;

        // Регистрация всех типов из сборки, которые заканчиваются на "Service", как реализации их интерфейсов.
        builder.RegisterAssemblyTypes(servicesAssembly)
            .Where(type => type.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();  // Каждое обращение в рамках одного запроса использует один экземпляр.
    }
}
