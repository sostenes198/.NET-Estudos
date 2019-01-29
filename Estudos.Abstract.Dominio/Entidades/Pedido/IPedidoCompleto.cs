using Estudos.Abstract.Dominio.Enuns;
using System;
using System.Collections.Generic;

namespace Estudos.Abstract.Dominio.Entidades.Pedido
{
    public interface IPedidoCompleto: IEntidade
    {
        int CodigoMesa { get; set; }

        string NomeMesa { get; set; }

        DateTime Data { get; set; }

        decimal ValorTotal { get; set; }

        TipoPagamentoEnum TipoPagamento { get; set; }

        decimal? ValorTroco { get; set; }

        IEnumerable<IPedido> Pedidos { get; set; }
    }
}
