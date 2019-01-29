using System;
using Castle.MicroKernel.Registration;

namespace Estudos.Ioc.Ioc.Communs.Windsor
{
    public static class WindsorDepencyInjectorExtensions
    {
        public static ComponentRegistration<T> OverridesExistingRegistration<T>(this ComponentRegistration<T> componentRegistration) where T : class
        {
            return componentRegistration
                .Named(Guid.NewGuid().ToString())
                .IsDefault();
        }
    }
}