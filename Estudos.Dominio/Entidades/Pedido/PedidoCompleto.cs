using System;
using System.Collections.Generic;
using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Pedido;
using Estudos.Abstract.Dominio.Enuns;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Dominio.Entidades.Pedido
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class PedidoCompleto : AEntidade, IPedidoCompleto
    {
        public PedidoCompleto()
        { }

        public int CodigoMesa { get; set; }

        public string NomeMesa { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorTotal { get; set; }

        public TipoPagamentoEnum TipoPagamento { get; set; }

        public decimal? ValorTroco { get; set; }

        public IEnumerable<IPedido> Pedidos { get; set; }
    }
}
