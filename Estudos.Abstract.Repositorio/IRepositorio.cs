using Estudos.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Abstract.Repositorio
{
    public interface IRepositorio        
    {
        Task<T> ObterEntidadePorChavePrimaria<T>(params object[] list) where T : AEntidade;

        Task<List<T>> ObterTodasEntidades<T>() where T : AEntidade;

        Task<T> InserirEntidade<T>(T entidade) where T : AEntidade;

        Task<T> AtualizarEntidade<T>(T entidade) where T : AEntidade;

        Task ExcluirEntidade<T>(params object[] list) where T : AEntidade;
    } 
}
