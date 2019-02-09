using Estudos.Abstract.Servico.Servico_Cardapio;
using Estudos.Api.GraphQL.Queries;
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
        readonly ISchema _schema;

        public CardapioCategoriaGraphQLController(ICardapioCategoriaService service, IDocumentExecuter documentExecuter,
            ISchema schema)
        {
            _service = service;
            _documentExecuter = documentExecuter;
            _schema = schema;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                OperationName = query.OperationName,
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