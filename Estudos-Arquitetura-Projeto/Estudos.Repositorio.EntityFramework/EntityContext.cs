using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.Dominio.Entidades;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Pedido;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using Estudos.Repositorio.EntityFrameworkCore.Base;
using Estudos.Repositorio.EntityFrameworkCore.Extensoes;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore
{
    [LifeStyleAttribute(LifeStyleIoCEnum.Transient)]
    public class EntityContext : DbContext, IContext
    {
        public EntityContext()
        {
        }

        protected EntityContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<CardapioCategoria> CardapiosCategoria { get; set; }

        public DbSet<Cardapio> Cardapios { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<PedidoCompleto> PedidosCompleto { get; set; }

        public Task<T> ObterEntidadePorChavePrimaria<T>(params object[] list) where T : AEntidade
        {
            return Task.Run(() => Find<T>(list));
        }

        public Task<List<T>> ObterTodasEntidades<T>() where T : AEntidade
        {
            return Task.Run(() => Set<T>().ToList());
        }

        public Task<T> InserirEntidade<T>(T entidade) where T : AEntidade
        {
            return Task.Run(() =>
            {
                Set<T>().Add(entidade);
                SaveChanges();
                return entidade;
            });
        }

        public Task<T> AtualizarEntidade<T>(T entidade) where T : AEntidade
        {
            return Task.Run(() =>
            {
                var entidadeBanco = this.Find(entidade);
                Entry(entidadeBanco).CurrentValues.SetValues(entidade);
                Entry(entidadeBanco).State = EntityState.Modified;

                SaveChanges();

                return entidadeBanco;
            });
        }

        public Task ExcluirEntidade<T>(T entidade) where T : AEntidade
        {
            return Task.Run(() =>
            {
                var entidadeBanco = this.Find(entidade);
                Set<T>().Remove(entidadeBanco);
                SaveChanges();
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DefinirPrecisaoModelDecimal(18, 2);
            modelBuilder.CriarMapeamento();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer("Data Source=DESKTOP-QBS77K7\\SQLEXPRESS2017;Initial Catalog=Estudos;Integrated Security=False;User ID=sa;Password=123456");

            //base.OnConfiguring(optionsBuilder);
        }
    }
}