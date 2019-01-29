using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.Repositorio.EntityFrameworkCore.Mapeamento.Mapeamento_Cardapio
{
    public class CardapioCategoriaMap : AMap, IEntityTypeConfiguration<CardapioCategoria>
    {
        public void Configure(EntityTypeBuilder<CardapioCategoria> builder)
        {
            builder.HasKey(lnq => lnq.Codigo);

            builder.Property(lnq => lnq.Codigo)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(AEntidade.tamanhoStringPadrao);

            builder.ToTable("Cardapio_Categoria");
            builder.Property(t => t.Codigo).HasColumnName($"CardCat_{nameof(CardapioCategoria.Codigo)}");
            builder.Property(t => t.Descricao).HasColumnName($"CardCat_{nameof(CardapioCategoria.Descricao)}");
        }
    }
}