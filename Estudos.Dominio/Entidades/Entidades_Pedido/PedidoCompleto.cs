using System;
using System.Collections.Generic;
using Estudos.Dominio.Enuns;

namespace Estudos.Dominio.Entidades.Entidades_Pedido
{
    public class PedidoCompleto : AEntidade
    {
        public const int tamanhoStringMesa = 10;

        public PedidoCompleto()
        { }

        public int CodigoMesa { get; set; }

        public string NomeMesa { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorTotal { get; set; }

        public TipoPagamentoEnum TipoPagamento { get; set; }

        public decimal? ValorTroco { get; set; }

        public IEnumerable<Pedido> Pedidos { get; set; }
    }
}
