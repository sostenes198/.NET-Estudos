using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Abstract.Servico.Servcice_Cardapio
{
    public interface ICardapioCategoriaService: IBaseService
    {
        Task<IEnumerable<CardapioCategoriaDTO>> ObterTodosCardapiosCategoria();

        Task<CardapioCategoriaDTO> ObterCardapioCategoriaPorChavePrimaria(CardapioCategoriaDTO dto);
    }
}
