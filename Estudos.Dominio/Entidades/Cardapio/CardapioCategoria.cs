using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using System.Collections.Generic;

namespace Estudos.Dominio.Entidades.Cardapio
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class CardapioCategoria : AEntidade, ICardapioCategoria
    {
        public CardapioCategoria() { }

        public string Descricao { get; set; }

        public IEnumerable<ICardapio> Cardapios { get; set; }
    }
}
