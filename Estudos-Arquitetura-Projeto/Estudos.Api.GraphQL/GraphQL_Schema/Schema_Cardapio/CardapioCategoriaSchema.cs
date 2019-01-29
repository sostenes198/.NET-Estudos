using Estudos.Api.GraphQL.Mutations.Mutation_Cardapio;
using Estudos.Api.GraphQL.Queries.Query_Cardapio;
using GraphQL;
using GraphQL.Types;

namespace Estudos.Api.GraphQL.GraphQL_Schema.Schema_Cardapio
{
    public class CardapioCategoriaSchema : Schema
    {
        public CardapioCategoriaSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<CardapioCategoriaQuery>();
            Mutation = resolver.Resolve<CardapioCategoriaMutation>();
        }
    }
}