using System.Collections.Generic;
using Estudos.Dominio.Entidades.Entidades_Pedido;

namespace Estudos.Dominio.Entidades.Entidades_Cardapio
{
    public class Cardapio : AEntidade
    {
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public int CodigoCardapioCategoria { get; set; }

        public CardapioCategoria CardapioCategoria { get; set; }

        public IEnumerable<Pedido> Pedidos { get; set; }
    }
}