using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Pedido;
using Estudos.Abstract.Servico.DTOs.DTO_Pedido;
using Estudos.Abstract.Servico.Servico_Pedido;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Estudos.Global.Extensions;

namespace Estudos.Servico.Servico_Pedido
{
    public class PedidoCompletoService : IPedidoCompletoService
    {
        private readonly IPedidoCompletoRepositorio _pedidoCompletoRepositorio;

        public PedidoCompletoService(IPedidoCompletoRepositorio pedidoCompletoRepositorio)
        {
            _pedidoCompletoRepositorio = pedidoCompletoRepositorio;
        }

        public async Task<PedidoCompletoDTO> ObterPedidoCompletoPorChavePrimaria(PedidoCompletoDTO pedidoCompletoDTO)
        {
            var resultado = await _pedidoCompletoRepositorio.ObterEntidadePorChavePrimaria<PedidoCompleto>(pedidoCompletoDTO.Codigo);

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task<IEnumerable<PedidoCompletoDTO>> ObterTodosPedidosCompleto()
        {
            var resultado = await _pedidoCompletoRepositorio.ObterTodasEntidades<PedidoCompleto>();

            return resultado.Transformar<List<PedidoCompletoDTO>>();
        }

        public async Task<PedidoCompletoDTO> SalvarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            var resultado = await _pedidoCompletoRepositorio.InserirEntidade(pedidoCompletoDTO.Transformar<PedidoCompleto>());

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task<PedidoCompletoDTO> AtualizarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            var resultado = await _pedidoCompletoRepositorio.AtualizarEntidade(pedidoCompletoDTO.Transformar<PedidoCompleto>());

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task ExlcuirPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            await _pedidoCompletoRepositorio.ExcluirEntidade(pedidoCompletoDTO.Transformar<PedidoCompleto>());
        }
    }
}