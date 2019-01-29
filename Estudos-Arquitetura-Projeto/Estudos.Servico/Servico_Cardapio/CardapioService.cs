using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Abstract.Servico.Servico_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Extensions;

namespace Estudos.Servico.Servico_Cardapio
{
    [IoC]
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepositorio _repositorio;

        public CardapioService(ICardapioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<CardapioDTO> ObterCardapioPorChavePrimaria(CardapioDTO cardapioDTO)
        {
            var resultado = await _repositorio.ObterEntidadePorChavePrimaria<Cardapio>(cardapioDTO.Codigo);

            return resultado.Transformar<CardapioDTO>();
        }

        public async Task<IEnumerable<CardapioDTO>> ObterTodosCardapios()
        {
            var resultado = await _repositorio.ObterTodasEntidades<Cardapio>();

            return resultado.Transformar<List<CardapioDTO>>();
        }

        public async Task<CardapioDTO> SalvarCardapio(CardapioDTO cardapioDTO)
        {
            var resultado = await _repositorio.InserirEntidade(cardapioDTO.Transformar<Cardapio>());

            return resultado.Transformar<CardapioDTO>();
        }

        public async Task<CardapioDTO> AtualizarCardapio(CardapioDTO cardapioDTO)
        {
            var resultado = await _repositorio.AtualizarEntidade(cardapioDTO.Transformar<Cardapio>());

            return resultado.Transformar<CardapioDTO>();
        }

        public async Task ExlcuirCardapio(CardapioDTO cardapioDTO)
        {
            await _repositorio.ExcluirEntidade(cardapioDTO.Transformar<Cardapio>());
        }
    }
}