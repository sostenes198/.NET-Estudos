using System.ComponentModel.DataAnnotations;

namespace Estudos.Abstract.Dominio.Enuns
{
    public enum TipoPagamentoEnum
    {
        [Display(Name = "Cartão")]
        Cartao = 1,

        [Display(Name = "Dinheiro")]
        Dinheiro = 2,
    }
}
