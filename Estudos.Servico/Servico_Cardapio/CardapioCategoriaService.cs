using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Abstract.Servico.Servico_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Servico.Servico_Cardapio
{
    [IoC]
    public class CardapioCategoriaService : ICardapioCategoriaService
    {
        readonly ICardapioCategoriaRepositorio _repositorio;

        public CardapioCategoriaService(ICardapioCategoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<CardapioCategoriaDTO> ObterCardapioCategoriaPorChavePrimaria(CardapioCategoriaDTO dto)
        {
            CardapioCategoria resultado = await _repositorio.ObterEntidadePorChavePrimaria<CardapioCategoria>(dto.Codigo);

            return resultado.Transformar<CardapioCategoriaDTO>();
        }

        public async Task<IEnumerable<CardapioCategoriaDTO>> ObterTodosCardapiosCategoria()
        {
            List<CardapioCategoria> resultado = await _repositorio.ObterTodasEntidades<CardapioCategoria>();

            return resultado.Transformar<List<CardapioCategoriaDTO>>();
        }

        public async Task<CardapioCategoriaDTO> SalvarCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO)
        {
            CardapioCategoria resultado = await _repositorio.InserirEntidade(cardapioCategoriaDTO.Transformar<CardapioCategoria>());

            return resultado.Transformar<CardapioCategoriaDTO>();
        }

        public async Task<CardapioCategoriaDTO> AtualizarCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO)
        {
            CardapioCategoria resultado = await _repositorio.AtualizarEntidade(cardapioCategoriaDTO.Transformar<CardapioCategoria>());

            return resultado.Transformar<CardapioCategoriaDTO>();
        }

        public async Task ExlcuirCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO)
        {
            await _repositorio.ExcluirEntidade(cardapioCategoriaDTO.Transformar<CardapioCategoria>());
        }
    }
}
