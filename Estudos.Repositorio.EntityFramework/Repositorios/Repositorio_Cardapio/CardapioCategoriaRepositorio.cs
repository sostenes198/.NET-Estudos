using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Global.Atributos;

namespace Estudos.Repositorio.EntityFrameworkCore.Repositorio_Cardapio
{
    [IoC]
    public class CardapioCategoriaRepositorio : EntityContext, ICardapioCategoriaRepositorio
    { }
}
