using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Repositorio.EntityFrameworkCore.Extensoes;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore.Context
{
    public abstract partial class EntityContext : ARepositorioEntity        
    {       
        public override Task<T> ObterEntidadePorChavePrimaria<T>(params object[] list)
        {
            return this.FindAsync<T>(list);
        }

        public override Task<List<T>> ObterTodasEntidades<T>()
        {
            return Set<T>().ToListAsync();
        }

        public override async Task<T> InserirEntidade<T>(T entidade)
        {
            Set<T>().Add(entidade);
            await SaveChangesAsync();
            return entidade;
        }

        public override async Task<T> AtualizarEntidade<T>(T entidade)
        {
            T entidadeBanco = await this.FindAsync(entidade);
            Entry(entidadeBanco).CurrentValues.SetValues(entidade);
            Entry(entidadeBanco).State = EntityState.Modified;

            await SaveChangesAsync();

            return entidadeBanco;
        }

        public override async Task ExcluirEntidade<T>(params object[] list)
        {
            var entidadeBanco = await this.FindAsync<T>(list);
            Set<T>().Remove(entidadeBanco);
            await SaveChangesAsync();
        }            
    }
}
