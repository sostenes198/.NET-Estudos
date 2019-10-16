using Estudos.Ioc.Ioc.Abstractions.DI;
using Estudos.Ioc.Services.Abstractions.Services;
using Estudos.Ioc.Services.Services;

namespace Estudos.Ioc.Services
{
    public static class Bootstrapper
    {
        public static ADependencyInjector AddServices(this ADependencyInjector injector)
        {
            injector.RegisterDependencyTransient<IInicializadorLancadorHorasService, InicializadorLancadorHorasService>();
            injector.RegisterDependencyTransient<ITemplate, Template1>();
            injector.RegisterDependencyTransient<ITemplate, Template2>();


            return injector;
        }
    }
}