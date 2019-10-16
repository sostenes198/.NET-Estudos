using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;

namespace Estudos.Abstract.Servico.Servico_Cardapio
{
    public interface ICardapioCategoriaService : IBaseService
    {
        Task<IEnumerable<CardapioCategoriaDTO>> ObterTodosCardapiosCategoria();

        Task<CardapioCategoriaDTO> ObterCardapioCategoriaPorChavePrimaria(CardapioCategoriaDTO cardapioCategoriaDTO);

        Task<CardapioCategoriaDTO> SalvarCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO);

        Task<CardapioCategoriaDTO> AtualizarCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO);

        Task ExlcuirCardapioCategoria(CardapioCategoriaDTO cardapioCategoriaDTO);
    }
}