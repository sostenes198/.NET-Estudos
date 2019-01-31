using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.Abstract.Dominio.Entidades;
using Estudos.Abstract.Repositorio;
using Microsoft.EntityFrameworkCore;

namespace Estudos.Repositorio.EntityFrameworkCore
{
    public abstract class ARepositorioEntity : DbContext, IRepositorio
    {
        public ARepositorioEntity()
            : base()
        { }

        public ARepositorioEntity(DbContextOptions options)
            : base(options)
        { }

        public abstract Task<T> AtualizarEntidade<T>(T entidade) where T : AEntidade;
        public abstract Task ExcluirEntidade<T>(params object[] list) where T : AEntidade;
        public abstract Task<T> InserirEntidade<T>(T entidade) where T : AEntidade;
        public abstract Task<T> ObterEntidadePorChavePrimaria<T>(params object[] list) where T : AEntidade;
        public abstract Task<List<T>> ObterTodasEntidades<T>() where T : AEntidade;
    }
}
