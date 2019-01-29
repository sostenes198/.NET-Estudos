using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Mapeamento.Mapeamento_Cardapio
{
    public class CardapioMap : AMap, IEntityTypeConfiguration<Cardapio>
    {
        public void Configure(EntityTypeBuilder<Cardapio> builder)
        {
            builder.HasKey(lnq => new {lnq.Codigo});

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(AEntidade.tamanhoStringPadrao);

            builder.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(AEntidade.tamanhoStringPadrao);

            builder.ToTable("Cardapio");
            builder.Property(t => t.Codigo).HasColumnName($"Card_{nameof(Cardapio.Codigo)}");
            builder.Property(t => t.Titulo).HasColumnName($"Card_{nameof(Cardapio.Titulo)}");
            builder.Property(t => t.Descricao).HasColumnName($"Card_{nameof(Cardapio.Descricao)}");
            builder.Property(t => t.CodigoCardapioCategoria).HasColumnName($"CardCat_{nameof(Cardapio.Codigo)}");

            builder.HasOne(lnq => lnq.CardapioCategoria)
                .WithMany(lnq => lnq.Cardapios)
                .HasForeignKey(lnq => lnq.CodigoCardapioCategoria);
        }
    }
}