using Estudos.Abstract.Dominio.Entidades.Cardapio;

namespace Estudos.Abstract.Dominio.Entidades.Pedido
{
    public interface IPedido : IEntidade
    {
        string Observacao { get; set; }

        int Quantidade { get; set; }

        decimal ValorTotalPedido { get; set; }

        int CodigoCardapio { get; set; }

        int CodigoPedidoCompleto { get; set; }

        ICardapio Cardapio { get; set; }       

        IPedidoCompleto PedidoCompleto { get; set; }
    }
}
