using Estudos.Abstract.Repositorio;
using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Estudos.Repositorio.EntityFrameworkCore.Base;
using Estudos.Repositorio.EntityFrameworkCore.Extensoes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Repositorio.EntityFrameworkCore
{
    public partial class EntityContext : DbContext, IRepositorio, IContext
    {
        public EntityContext()
            : base()
        { }

        public EntityContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<CardapioCategoria> CardapiosCategoria { get; set; }

        public DbSet<Cardapio> Cardapios { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<PedidoCompleto> PedidosCompleto { get; set; }

        public Task<T> ObterEntidadePorChavePrimaria<T>(params object[] list) where T : AEntidade
        {
            return FindAsync<T>(list);
        }

        public Task<List<T>> ObterTodasEntidades<T>() where T : AEntidade
        {
            return Set<T>().ToListAsync();
        }

        public async Task<T> InserirEntidade<T>(T entidade) where T : AEntidade
        {
            Set<T>().Add(entidade);
            await SaveChangesAsync();
            return entidade;
        }

        public async Task<T> AtualizarEntidade<T>(T entidade) where T : AEntidade
        {
            T entidadeBanco = await this.FindAsync(entidade);
            Entry(entidadeBanco).CurrentValues.SetValues(entidade);
            Entry(entidadeBanco).State = EntityState.Modified;

            await SaveChangesAsync();

            return entidadeBanco;
        }

        public async Task ExcluirEntidade<T>(params object[] list) where T : AEntidade
        {
            var entidadeBanco = await this.FindAsync<T>(list);
            Set<T>().Remove(entidadeBanco);
            await SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DefinirPrecisaoModelDecimal(18, 2);
            modelBuilder.CriarMapeamento();

            base.OnModelCreating(modelBuilder);
        }
    }
}
