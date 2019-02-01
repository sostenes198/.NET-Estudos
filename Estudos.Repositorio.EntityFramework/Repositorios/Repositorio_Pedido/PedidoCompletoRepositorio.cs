using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Pedido;
using Estudos.Global.Atributos;

namespace Estudos.Repositorio.EntityFrameworkCore.Repositorios.Repositorio_Pedido
{
    [IoC]
    public class PedidoCompletoRepositorio : EntityContext, IPedidoCompletoRepositorio
    { }
}
