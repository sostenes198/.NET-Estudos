using Estudos.Abstract.Servico.DTOs.DTO_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using GraphQL.Types;

namespace Estudos.Api.GraphQL.Types.Type_Cardapio
{
    [IoC(LifeStyleIoCEnum.Singleton)]
    public class CardapioCategoriaType : ObjectGraphType<CardapioCategoriaDTO>
    {
        public CardapioCategoriaType()
        {
            Name = "CardapioCategoria";

            Field(lnq => lnq.Codigo).Description("Código");
            Field(lnq => lnq.Descricao).Description("Descrição");
        }
    }
}