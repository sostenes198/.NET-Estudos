using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Mapeamento.Mapeamento_Pedido
{
    public class PedidoMap : AMap, IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(lnq => lnq.Codigo);

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Observacao)
                .IsRequired()
                .HasMaxLength(AEntidade.tamanhoStringPadrao);

            builder.ToTable("Pedido");
            builder.Property(t => t.Codigo).HasColumnName($"Ped_{nameof(Pedido.Codigo)}");
            builder.Property(t => t.Observacao).HasColumnName($"Ped_{nameof(Pedido.Observacao)}");
            builder.Property(t => t.ValorTotalPedido).HasColumnName($"Ped_{nameof(Pedido.ValorTotalPedido)}");
            builder.Property(t => t.CodigoCardapio).HasColumnName($"Card_{nameof(Cardapio.Codigo)}");
            builder.Property(t => t.CodigoPedidoCompleto).HasColumnName($"PedComp_{nameof(PedidoCompleto.Codigo)}");

            builder.HasOne(lnq => lnq.Cardapio)
                .WithMany(lnq => lnq.Pedidos)
                .HasForeignKey(lnq => lnq.CodigoCardapio);

            builder.HasOne(lnq => lnq.PedidoCompleto)
                .WithMany(lnq => lnq.Pedidos)
                .HasForeignKey(lnq => lnq.CodigoPedidoCompleto);
        }
    }
}