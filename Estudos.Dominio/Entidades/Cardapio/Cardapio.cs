using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Dominio.Entidades.Cardapio
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class Cardapio : AEntidade, ICardapio
    {
        public Cardapio() { }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public int CodigoCardapioCategoria { get; set; }

        public ICardapioCategoria CardapioCategoria { get; set; }
    }
}
