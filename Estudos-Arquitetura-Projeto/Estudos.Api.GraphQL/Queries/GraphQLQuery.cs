using Newtonsoft.Json.Linq;

namespace Estudos.Api.GraphQL.Queries
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }

        public string QueryName { get; set; }

        public string Query { get; set; }

        public JObject Variables { get; set; }
    }
}