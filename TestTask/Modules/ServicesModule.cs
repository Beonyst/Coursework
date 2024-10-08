#nullable enable
using API.Data.Services.Implementations;
using Autofac;
using Module = Autofac.Module;

namespace API.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var servicesAssembly = typeof(MedicineService).Assembly;

        builder.RegisterAssemblyTypes(servicesAssembly)
            .Where(type => type.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
