using Estudos.Abstract.Dominio.Entidades;
using System.Collections.Generic;

namespace Estudos.Abstract.Repositorio
{
    internal interface IRepositorio<T>
        where T: AEntidade
    {
        T ObterEntidadePorChavePrimaria(params object[] list);

        IEnumerable<T> ObterTodasEntidades(params object[] list);

        T InserirEntidade(T entidade);

        T AtualizarEntidade(T entidade);

        void ExcluirEntidade(params object[] list);
    } 
}
