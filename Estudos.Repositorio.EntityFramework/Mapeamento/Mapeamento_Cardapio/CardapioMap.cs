using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Repositorio.EntityFrameworkCore.Mapeamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Estudos.Repositorio.EntityFrameworkCore.Maps.Mapeamento_Cardapio
{
    public class CardapioMap : AMap, IEntityTypeConfiguration<Cardapio>
    {
        public void Configure(EntityTypeBuilder<Cardapio> builder)
        {
            builder.HasKey(lnq => new { lnq.Codigo });

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
                .WithMany(lnq => (IEnumerable<Cardapio>)lnq.Cardapios)
                .HasForeignKey(lnq => lnq.CodigoCardapioCategoria);
        }       
    }
}
