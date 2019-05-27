using System;
using System.Collections.Generic;
using System.Text;

namespace Estudos.Ioc.Ioc.Communs
{
    internal static class ContainerDependencyInjector<TInjector>
      where TInjector : class, new()
    {
        private static TInjector _container;

        public static TInjector Container
        {
            get
            {
                if(_container == null)
                    _container = new TInjector();

                return _container;
            }
        }
    }
}
