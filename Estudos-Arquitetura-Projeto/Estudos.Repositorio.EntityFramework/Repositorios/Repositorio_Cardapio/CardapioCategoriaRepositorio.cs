using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;

namespace Estudos.Repositorio.EntityFrameworkCore.Repositorios.Repositorio_Cardapio
{
    [IoC(LifeStyleIoCEnum.Transient)]
    public class CardapioCategoriaRepositorio : EntityContext, ICardapioCategoriaRepositorio
    {
    }
}