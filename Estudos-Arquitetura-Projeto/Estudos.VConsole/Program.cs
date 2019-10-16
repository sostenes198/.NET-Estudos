using Estudos.Abstract.Repositorio.Repositorios.Repositorio_Cardapio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Estudos.IoC;
using SimpleInjector.Lifestyles;

namespace Estudos.VConsole
{
    internal class Program
    {
        private static void Main()
        {
            var container = IoCSimpleInjector.InjetarDependencias();

            using (AsyncScopedLifestyle.BeginScope(container))
            {
                var repositorioGenerico = container.GetInstance<ICardapioCategoriaRepositorio>();

                var entidade = new CardapioCategoria
                {
                    Descricao = "asdasd"
                };

                repositorioGenerico.InserirEntidade(entidade);
            }
        }
    }
}