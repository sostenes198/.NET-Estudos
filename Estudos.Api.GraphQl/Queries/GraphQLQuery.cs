using Newtonsoft.Json.Linq;

namespace Estudos.Api.GraphQL.Queries
{
    public class GraphQLQuery
    {
        public string NomeOperacao { get; set; }

        public string NomeQuery { get; set; }

        public string Query { get; set; }

        public JObject Variaveis { get; set; }
    }
}
