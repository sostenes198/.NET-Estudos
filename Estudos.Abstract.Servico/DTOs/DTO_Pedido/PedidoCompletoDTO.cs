using Estudos.Global.Enuns.Entidades;
using System;

namespace Estudos.Abstract.Servico.DTOs.DTO_Pedido
{
    public class PedidoCompletoDTO
    {
        public PedidoCompletoDTO()
        { }

        public int Codigo { get; set; }

        public int CodigoMesa { get; set; }

        public string NomeMesa { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorTotal { get; set; }

        public TipoPagamentoEnum TipoPagamento { get; set; }

        public decimal? ValorTroco { get; set; }

    }
}
