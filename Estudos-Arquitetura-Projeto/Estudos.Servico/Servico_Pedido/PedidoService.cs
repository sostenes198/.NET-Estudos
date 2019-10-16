using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Pedido;
using Estudos.Abstract.Servico.DTOs.DTO_Pedido;
using Estudos.Abstract.Servico.Servico_Pedido;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Estudos.Global.Atributos;
using Estudos.Global.Extensions;

namespace Estudos.Servico.Servico_Pedido
{
    [IoC]
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public PedidoService(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task<PedidoDTO> ObterPedidoPorChavePrimaria(PedidoDTO pedidoDTO)
        {
            var resultado = await _pedidoRepositorio.ObterEntidadePorChavePrimaria<Pedido>(pedidoDTO.Codigo);

            return resultado.Transformar<PedidoDTO>();
        }

        public async Task<IEnumerable<PedidoDTO>> ObterTodosPedidos()
        {
            var resultado = await _pedidoRepositorio.ObterTodasEntidades<Pedido>();

            return resultado.Transformar<List<PedidoDTO>>();
        }

        public async Task<PedidoDTO> SalvarPedido(PedidoDTO pedidoDTO)
        {
            var resultado = await _pedidoRepositorio.InserirEntidade(pedidoDTO.Transformar<Pedido>());

            return resultado.Transformar<PedidoDTO>();
        }

        public async Task<PedidoDTO> AtualizarPedido(PedidoDTO pedidoDTO)
        {
            var resultado = await _pedidoRepositorio.AtualizarEntidade(pedidoDTO.Transformar<Pedido>());

            return resultado.Transformar<PedidoDTO>();
        }

        public async Task ExlcuirPedido(PedidoDTO pedidoDTO)
        {
            await _pedidoRepositorio.ExcluirEntidade(pedidoDTO.Transformar<Pedido>());
        }
    }
}