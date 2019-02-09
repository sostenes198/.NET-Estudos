using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Pedido;
using Estudos.Abstract.Servico.DTOs.DTO_Pedido;
using Estudos.Abstract.Servico.Servico_Pedido;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Estudos.Global.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Estudos.Servico.Servico_Pedido
{
    public class PedidoCompletoService : IPedidoCompletoService
    {
        readonly IPedidoCompletoRepositorio _pedidoCompletoRepositorio;

        public PedidoCompletoService(IPedidoCompletoRepositorio pedidoCompletoRepositorio)
        {
            _pedidoCompletoRepositorio = pedidoCompletoRepositorio;
        }

        public async Task<PedidoCompletoDTO> ObterPedidoCompletoPorChavePrimaria(PedidoCompletoDTO pedidoCompletoDTO)
        {
            PedidoCompleto resultado = await _pedidoCompletoRepositorio.ObterEntidadePorChavePrimaria<PedidoCompleto>(pedidoCompletoDTO.Codigo);

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task<IEnumerable<PedidoCompletoDTO>> ObterTodosPedidosCompleto()
        {
            List<PedidoCompleto> resultado = await _pedidoCompletoRepositorio.ObterTodasEntidades<PedidoCompleto>();

            return resultado.Transformar<List<PedidoCompletoDTO>>();
        }

        public async Task<PedidoCompletoDTO> SalvarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            PedidoCompleto resultado = await _pedidoCompletoRepositorio.InserirEntidade<PedidoCompleto>(pedidoCompletoDTO.Transformar<PedidoCompleto>());

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task<PedidoCompletoDTO> AtualizarPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            PedidoCompleto resultado = await _pedidoCompletoRepositorio.AtualizarEntidade<PedidoCompleto>(pedidoCompletoDTO.Transformar<PedidoCompleto>());

            return resultado.Transformar<PedidoCompletoDTO>();
        }

        public async Task ExlcuirPedidoCompleto(PedidoCompletoDTO pedidoCompletoDTO)
        {
            await _pedidoCompletoRepositorio.ExcluirEntidade<PedidoCompleto>(pedidoCompletoDTO.Transformar<PedidoCompleto>());
        }     
    }
}
