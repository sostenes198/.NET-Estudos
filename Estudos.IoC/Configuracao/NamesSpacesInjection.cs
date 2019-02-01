using Estudos.Global.NameSpace;
using Estudos.Global.NameSpace.Definicao;
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
                    new NameSpaceDefinition(NameSpaceContant.AbstractRepositorio, NameSpaceContant.ImplementacaoRepositorioEntityFrameworkCore)
                };
            }
        }        
    }
}
