using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore.Base
{
    public interface IContext
    {
        DbSet<CardapioCategoria> CardapiosCategoria { get; set; }

        DbSet<Cardapio> Cardapios { get; set; }

        DbSet<Pedido> Pedidos { get; set; }

        DbSet<PedidoCompleto> PedidosCompleto { get; set; }
    }
}
