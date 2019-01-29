using Estudos.Ioc.Ioc.Abstractions.DI;
using Estudos.Ioc.Ioc.DI.Windsor;
using Estudos.Ioc.Services;

namespace Estudos.Ioc.Ioc
{
    public static class Bootstrapper
    {
        public static ADependencyInjector AddIoc()
        {
            var container = new WindsorDependencyInjector();

            container.AddServices();


            return container;
        }
    }
}