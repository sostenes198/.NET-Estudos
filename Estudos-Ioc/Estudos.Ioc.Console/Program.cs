using Estudos.Ioc.Ioc;
using Estudos.Ioc.Ioc.Abstractions.DI;
using Estudos.Ioc.Ioc.DI.Windsor;
using Estudos.Ioc.Services.Abstractions.Services;
using System;

namespace Estudos.Ioc.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Bootstrapper.AddIoc();
            container.RegisterDepedencySingleton<ADependencyInjector, WindsorDependencyInjector>();

            var x = container.Resolve<ITemplate>();
        }
    }
}
