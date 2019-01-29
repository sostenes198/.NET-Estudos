using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Abstract.Servico.Servico_Cardapio;
using Estudos.Api.GraphQL.Types.Type_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using GraphQL.Types;

namespace Estudos.Api.GraphQL.Queries.Query_Cardapio
{
    [IoC(LifeStyleIoCEnum.Singleton)]
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

            Field<CardapioCategoriaType>(
                "cardapioCategoriaPorCodigo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "codigo"}
                ),
                resolve: contexto =>
                {
                    var cardapioCategoriaDTO = new CardapioCategoriaDTO
                    {
                        Codigo = contexto.GetArgument<int>("codigo")
                    };

                    var cardapioCategorias = service.ObterCardapioCategoriaPorChavePrimaria(cardapioCategoriaDTO);

                    return cardapioCategorias;
                }
            );
        }
    }
}