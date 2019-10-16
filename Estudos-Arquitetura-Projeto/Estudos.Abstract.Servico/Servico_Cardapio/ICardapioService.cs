using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;

namespace Estudos.Abstract.Servico.Servico_Cardapio
{
    public interface ICardapioService : IBaseService
    {
        Task<IEnumerable<CardapioDTO>> ObterTodosCardapios();

        Task<CardapioDTO> ObterCardapioPorChavePrimaria(CardapioDTO CardapioDTO);

        Task<CardapioDTO> SalvarCardapio(CardapioDTO CardapioDTO);

        Task<CardapioDTO> AtualizarCardapio(CardapioDTO CardapioDTO);

        Task ExlcuirCardapio(CardapioDTO CardapioDTO);
    }
}