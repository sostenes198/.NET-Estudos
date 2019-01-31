using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Maps.Cardapio
{
    public class CardapioMap : IEntityTypeConfiguration<ICardapio>
    {
        public void Configure(EntityTypeBuilder<ICardapio> builder)
        {
            builder.HasKey(lnq => lnq.Codigo);

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(AEntidade.TamanhoStringPadrao);

            builder.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(AEntidade.TamanhoStringPadrao);

            builder.ToTable("Cardapio");
            builder.Property(t => t.Codigo).HasColumnName("Card_Codigo");
            builder.Property(t => t.Titulo).HasColumnName("Card_Titulo");
            builder.Property(t => t.Descricao).HasColumnName("Card_Descricao");
            builder.Property(t => t.CodigoCardapioCategoria).HasColumnName("CardCat_Codigo");

            builder.HasOne(lnq => lnq.CardapioCategoria)
                .WithMany(lnq => lnq.Cardapios)
                .HasForeignKey(lnq => lnq.CodigoCardapioCategoria);

        }
    }
}
