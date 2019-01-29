namespace Estudos.Abstract.Dominio.Entidades.Cardapio
{
    public interface ICardapio: IEntidade
    {
        string Titulo { get; set; }

        string Descricao { get; set; }

        int CodigoCardapioCategoria { get; set; }

        ICardapioCategoria CardapioCategoria { get; set; }
    }
}
