using Estudos.Abstract.Servico.DTOs.DTO_Pedido;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Abstract.Servico.Servico_Pedido
{
    public interface IPedidoService : IBaseService
    {
        Task<IEnumerable<PedidoDTO>> ObterTodosPedidos();

        Task<PedidoDTO> ObterPedidoPorChavePrimaria(PedidoDTO pedidoDTO);

        Task<PedidoDTO> SalvarPedido(PedidoDTO pedidoDTO);

        Task<PedidoDTO> AtualizarPedido(PedidoDTO pedidoDTO);

        Task ExlcuirPedido(PedidoDTO pedidoDTO);
    }
}
