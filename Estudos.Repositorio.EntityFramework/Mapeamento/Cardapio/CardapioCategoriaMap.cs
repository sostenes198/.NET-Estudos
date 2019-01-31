using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Dominio.Entidades.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Mapeamento.Cardapio
{
    public class CardapioCategoriaMap : IEntityTypeConfiguration<ICardapioCategoria>
    {
        public void Configure(EntityTypeBuilder<ICardapioCategoria> builder)
        {
            builder.HasKey(lnq => lnq.Codigo);

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(AEntidade.TamanhoStringPadrao);

            builder.ToTable("Cardapio_Categoria");
            builder.Property(t => t.Codigo).HasColumnName("CardCat_Codigo");
            builder.Property(t => t.Descricao).HasColumnName("CardCat_Descricao");
        }
    }
}
