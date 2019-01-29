using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Cardapio;
using Estudos.Abstract.Dominio.Entidades.Pedido;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Dominio.Entidades.Pedido
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class Pedido : AEntidade, IPedido
    {
        public Pedido()
        {}

        public string Observacao { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorTotalPedido { get; set; }

        public int CodigoCardapio { get; set; }

        public int CodigoPedidoCompleto { get; set; }

        public ICardapio Cardapio { get; set; }

        public IPedidoCompleto PedidoCompleto { get; set; }
    }
}
