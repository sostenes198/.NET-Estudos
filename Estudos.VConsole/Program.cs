using Estudos.Abstract.Dominio.Entidades.Pedido;
using Estudos.Dominio.Entidades.Pedido;
using Estudos.IoC;
using SimpleInjector;
using System;

namespace Estudos.VConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Container _container = IoCSimpleInjector.InjetarDependencias();

            IPedido _pedido = _container.GetInstance<Pedido>();

            Console.WriteLine("Hello World!");
        }
    }
}
