#nullable enable
using API.Data.Implementations;
using API.Data.Interfaces;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace API.Modules;

// Модуль, который регистрирует репозитории и единицу работы (Unit of Work) в контейнере зависимостей Autofac.
public class RepositoriesModule : Module
{
    // Переопределённый метод для регистрации компонентов в контейнере.
    protected override void Load(ContainerBuilder builder)
    {
        // Регистрация обобщённого репозитория как реализации IGenericRepository.
        builder.RegisterGeneric(typeof(EFGenericRepsitory<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();  // Каждое обращение в рамках одного запроса использует один экземпляр.

        // Регистрация UnitOfWork как реализации IUnitOfWork.
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();  // Каждое обращение в рамках одного запроса использует один экземпляр.

        // Получение сборки репозитория для динамической регистрации всех классов, заканчивающихся на "Repository".
        var repositoryAssembly = typeof(EFGenericRepsitory<>).GetTypeInfo().Assembly;

        // Регистрация всех типов из сборки, которые заканчиваются на "Repository", как реализации их интерфейсов.
        builder.RegisterAssemblyTypes(repositoryAssembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();  // Каждое обращение в рамках одного запроса использует один экземпляр.
    }
}
