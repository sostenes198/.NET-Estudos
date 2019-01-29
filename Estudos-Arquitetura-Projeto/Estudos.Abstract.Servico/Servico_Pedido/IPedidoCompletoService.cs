using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Servico.DTOs.DTO_Pedido;

namespace Estudos.Abstract.Servico.Servico_Pedido
{
    public interface IPedidoCompletoService : IBaseService
    {
        Task<IEnumerable<PedidoCompletoDTO>> ObterTodosPedidosCompleto();

        Task<PedidoCompletoDTO> ObterPedidoCompletoPorChavePrimaria(PedidoCompletoDTO pedidoCompletoDTO);

        Task<PedidoCompletoDTO> SalvarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO);

        Task<PedidoCompletoDTO> AtualizarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO);

        Task ExlcuirPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO);
    }
}