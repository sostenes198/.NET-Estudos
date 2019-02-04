using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Repositorio.EntityFrameworkCore.Repositorio_Cardapio
{
    [IoC(LifeStyleIoCEnum.Scoped)]
    public class CardapioCategoriaRepositorio : EntityContext, ICardapioCategoriaRepositorio
    { }
}
