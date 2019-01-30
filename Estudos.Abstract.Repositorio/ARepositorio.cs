using System.Collections.Generic;
using Estudos.Abstract.Dominio.Entidades;

namespace Estudos.Abstract.Repositorio
{
    public abstract class ARepositorio<T> : IRepositorio<T>
        where T : AEntidade
    {
        public ARepositorio()
        {}

        public abstract T AtualizarEntidade(T entidade);
        public abstract void ExcluirEntidade(params object[] list);
        public abstract T InserirEntidade(T entidade);
        public abstract T ObterEntidadePorChavePrimaria(params object[] list);
        public abstract IEnumerable<T> ObterTodasEntidades(params object[] list);
    }
}
