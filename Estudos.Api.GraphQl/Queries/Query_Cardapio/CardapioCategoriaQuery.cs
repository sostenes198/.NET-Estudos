using Estudos.Abstract.Servico.Servcice_Cardapio;
using Estudos.Api.GraphQL.Types.Type_Cardapio;
using GraphQL.Types;

namespace Estudos.Api.GraphQL.Queries.Query_Cardapio
{
    public class CardapioCategoriaQuery : ObjectGraphType
    {
        public CardapioCategoriaQuery(ICardapioCategoriaService service)
        {
            Field<ListGraphType<CardapioCategoriaType>>(
                "cardapioCategorias",
                resolve: contexto =>
                {
                    var cardapioCategorias = service.ObterTodosCardapiosCategoria();
                    return cardapioCategorias;
                }
            );

            Field<ListGraphType<CardapioCategoriaType>>(
                "cardapioCategoriaPorCodigo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>() { Name = "Codigo" }
                ),
                resolve: contexto =>
                {
                    var cardapioCategorias = service.ObterTodosCardapiosCategoria();
                    return cardapioCategorias;
                }
            );

        }
    }
}
