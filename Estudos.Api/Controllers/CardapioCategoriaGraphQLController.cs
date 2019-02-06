using Estudos.Abstract.Servico.Servcice_Cardapio;
using Estudos.Api.GraphQL.Queries;
using Estudos.Api.GraphQL.Queries.Query_Cardapio;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Estudos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardapioCategoriaGraphQLController : ControllerBase
    {
        readonly ICardapioCategoriaService _service;
        readonly IDocumentExecuter _documentExecuter;

        public CardapioCategoriaGraphQLController(ICardapioCategoriaService service, IDocumentExecuter documentExecuter)
        {
            _service = service;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variaveis.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = new Schema() { Query = new CardapioCategoriaQuery(_service) },
                Query = query.Query,
                OperationName = query.NomeOperacao,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}