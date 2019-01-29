using System.Collections.Generic;

namespace Estudos.Abstract.Dominio.Entidades.Cardapio
{
    public interface ICardapioCategoria: IEntidade
    {
        string Descricao { get; set; }

        IEnumerable<ICardapio> Cardapios { get; set; }
    }
}
