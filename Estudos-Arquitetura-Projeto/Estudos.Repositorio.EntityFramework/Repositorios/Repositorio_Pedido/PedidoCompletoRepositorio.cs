using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Pedido;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Repositorio.EntityFrameworkCore.Repositorios.Repositorio_Pedido
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class PedidoCompletoRepositorio : EntityContext, IPedidoCompletoRepositorio
    {
    }
}