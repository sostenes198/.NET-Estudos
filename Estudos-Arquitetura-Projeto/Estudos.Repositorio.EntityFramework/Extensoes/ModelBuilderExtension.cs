using System.Linq;
using System.Reflection;
using Estudos.Global.NameSpace;
using Estudos.Repositorio.EntityFrameworkCore.Mapeamento;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore.Extensoes
{
    public static class ModelBuilderExtension
    {
        public static void DefinirPrecisaoModelDecimal(this ModelBuilder modelBuilder, int escala, int precisao)
        {
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
                property.Relational().ColumnType = $"decimal({escala}, {precisao})";
        }

        public static void CriarMapeamento(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(NameSpaceContant.ImplementacaoRepositorioEntityFrameworkCore),
                lnq => lnq.IsClass && !lnq.IsAbstract && typeof(AMap).IsAssignableFrom(lnq));
        }
    }
}