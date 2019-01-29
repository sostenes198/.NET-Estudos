using Estudos.Ioc.Ioc;
using Estudos.Ioc.Ioc.Abstractions.DI;
using Estudos.Ioc.Ioc.DI.Windsor;
using Estudos.Ioc.Services.Abstractions.Services;

namespace Estudos.Ioc.Console
{
    internal class Program
    {
        private static void Main()
        {
            var container = Bootstrapper.AddIoc();
            container.RegisterDepedencySingleton<ADependencyInjector, WindsorDependencyInjector>();

            container.Resolve<ITemplate>();
        }
    }
}