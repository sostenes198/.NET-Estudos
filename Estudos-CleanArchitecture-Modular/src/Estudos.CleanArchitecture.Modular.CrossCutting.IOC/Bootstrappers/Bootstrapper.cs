using Microsoft.Extensions.DependencyInjection;

namespace Estudos.CleanArchitecture.Modular.CrossCutting.IOC.Bootstrappers;

public static class Bootstrapper
{
    public static IServiceCollection InitializeApplication(this IServiceCollection services)
    {
        return services
           .InitializeCommonsModules()
           .InitializeModuleSignature()
           .InitializeModuleSignatureACL();
    }
}