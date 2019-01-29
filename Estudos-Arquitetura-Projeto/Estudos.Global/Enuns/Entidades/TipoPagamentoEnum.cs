using System.ComponentModel.DataAnnotations;

namespace Estudos.Global.Enuns.Entidades
{
    public enum TipoPagamentoEnum
    {
        [Display(Name = "Cartão")] Cartao = 1,

        [Display(Name = "Dinheiro")] Dinheiro = 2
    }
}