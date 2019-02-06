using Estudos.Abstract.Repositorio;
using Estudos.Global.NameSpace;
using Estudos.Global.NameSpace.Definicao;
using Estudos.Repositorio.EntityFrameworkCore;
using System.Collections.Generic;

namespace Estudos.IoC.Configuracao
{
    public static class NamesSpacesInjection
    {
        public static List<NameSpaceDefinition> NamesSpaces
        {
            get
            {
                return new List<NameSpaceDefinition>()
                {
                    new NameSpaceDefinition(null, NameSpaceContant.ImplementacaoGraphQL),
                    new NameSpaceDefinition(NameSpaceContant.AbstractRepositorio, NameSpaceContant.ImplementacaoRepositorioEntityFrameworkCore),
                    new NameSpaceDefinition(NameSpaceContant.AbstractService, NameSpaceContant.ImplementacaoService),
                    new NameSpaceDefinition(typeof(IRepositorio), typeof(EntityContext))
                };
            }
        }        
    }
}
