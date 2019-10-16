using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Abstract.Servico.Servico_Cardapio;
using Estudos.Api.GraphQL.Types.Type_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using GraphQL.Types;

namespace Estudos.Api.GraphQL.Mutations.Mutation_Cardapio
{
    [IoC(LifeStyleIoCEnum.Singleton)]
    public class CardapioCategoriaMutation : ObjectGraphType
    {
        public CardapioCategoriaMutation(ICardapioCategoriaService _cardapioCategoriaService)
        {
            Name = "CardapioCategoriaMutation";

            Field<CardapioCategoriaType>(
                "criarCardapioCategoria",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CardapioCategoriaType>> {Name = "cardapioCategoria"}
                ),
                resolve: context =>
                {
                    var cardapioCategoriaDTO = context.GetArgument<CardapioCategoriaDTO>("cardapioCategoria");

                    return _cardapioCategoriaService.SalvarCardapioCategoria(cardapioCategoriaDTO);
                }
            );
        }
    }
}