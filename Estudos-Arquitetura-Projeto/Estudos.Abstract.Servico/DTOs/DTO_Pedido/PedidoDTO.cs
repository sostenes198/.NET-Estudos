namespace Estudos.Abstract.Servico.DTOs.DTO_Pedido
{
    public class PedidoDTO
    {
        public int Codigo { get; set; }

        public string Observacao { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorTotalPedido { get; set; }

        public int CodigoCardapio { get; set; }

        public int CodigoPedidoCompleto { get; set; }
    }
}