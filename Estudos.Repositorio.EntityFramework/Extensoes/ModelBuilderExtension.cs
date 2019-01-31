using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            {
                property.Relational().ColumnType = $"decimal(${escala}, ${precisao})";
            }
        }
    }
}
