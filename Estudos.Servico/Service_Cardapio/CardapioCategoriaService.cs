using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Estudos.Abstract.Repositorio;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Abstract.Servico.Servcice_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Global.Atributos;

namespace Estudos.Servico.Service_Cardapio
{
    [IoC]
    public class CardapioCategoriaService : ICardapioCategoriaService
    {
        readonly IRepositorio _repositorio;
        readonly IMapper _mapper;

        public CardapioCategoriaService(IRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<CardapioCategoriaDTO> ObterCardapioCategoriaPorChavePrimaria(CardapioCategoriaDTO dto)
        {
            CardapioCategoria resultado = await _repositorio.ObterEntidadePorChavePrimaria<CardapioCategoria>(dto.Codigo);

            return _mapper.Map<CardapioCategoriaDTO>(resultado);
        }

        public async Task<IEnumerable<CardapioCategoriaDTO>> ObterTodosCardapiosCategoria()
        {
            List<CardapioCategoria> entidades = await _repositorio.ObterTodasEntidades<CardapioCategoria>();

            return _mapper.Map<IEnumerable<CardapioCategoriaDTO>>(entidades);
        }
    }
}
