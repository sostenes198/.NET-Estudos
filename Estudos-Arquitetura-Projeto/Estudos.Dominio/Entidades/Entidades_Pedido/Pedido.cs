using Estudos.Dominio.Entidades.Entidades_Cardapio;

namespace Estudos.Dominio.Entidades.Entidades_Pedido
{
    public class Pedido : AEntidade
    {
        public string Observacao { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorTotalPedido { get; set; }

        public int CodigoCardapio { get; set; }

        public int CodigoPedidoCompleto { get; set; }

        public Cardapio Cardapio { get; set; }

        public PedidoCompleto PedidoCompleto { get; set; }
    }
}