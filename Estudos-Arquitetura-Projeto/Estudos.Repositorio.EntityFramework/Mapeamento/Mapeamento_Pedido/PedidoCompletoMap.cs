using Estudos.Dominio.Entidades.Entidades_Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Mapeamento.Mapeamento_Pedido
{
    public class PedidoCompletoMap : AMap, IEntityTypeConfiguration<PedidoCompleto>
    {
        public void Configure(EntityTypeBuilder<PedidoCompleto> builder)
        {
            builder.HasKey(lnq => lnq.Codigo);

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.NomeMesa)
                .IsRequired()
                .HasMaxLength(PedidoCompleto.tamanhoStringMesa);

            builder.ToTable("Pedido_Completo");
            builder.Property(t => t.Codigo).HasColumnName($"PedComp_{nameof(PedidoCompleto.Codigo)}");
            builder.Property(t => t.CodigoMesa).HasColumnName($"PedComp_{nameof(PedidoCompleto.CodigoMesa)}");
            builder.Property(t => t.Data).HasColumnName($"PedComp_{nameof(PedidoCompleto.Data)}");
            builder.Property(t => t.ValorTotal).HasColumnName($"PedComp_{nameof(PedidoCompleto.ValorTotal)}");
            builder.Property(t => t.TipoPagamento).HasColumnName($"PedComp_{nameof(PedidoCompleto.TipoPagamento)}");
            builder.Property(t => t.ValorTroco).HasColumnName($"PedComp_{nameof(PedidoCompleto.ValorTroco)}");
        }
    }
}