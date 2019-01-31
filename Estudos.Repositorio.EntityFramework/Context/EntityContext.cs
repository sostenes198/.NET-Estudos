using Estudos.Repositorio.EntityFrameworkCore.Extensoes;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore.Context
{
    public abstract partial class EntityContext
    {
        public EntityContext()
            : base()
        { }

        public EntityContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DefinirPrecisaoModelDecimal(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
