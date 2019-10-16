using System;
using System.Collections.Generic;
using Estudos.Global.Enuns.Entidades;

namespace Estudos.Dominio.Entidades.Entidades_Pedido
{
    public class PedidoCompleto : AEntidade
    {
        public const int tamanhoStringMesa = 10;

        public int CodigoMesa { get; set; }

        public string NomeMesa { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorTotal { get; set; }

        public TipoPagamentoEnum TipoPagamento { get; set; }

        public decimal? ValorTroco { get; set; }

        public IEnumerable<Pedido> Pedidos { get; set; }
    }
}