using Estudos.Abstract.Repositorio;
using Estudos.Dominio.Entidades.Entidades_Cardapio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.Api.GraphQl.Controllers
{    

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IRepositorio _repositorio;

        public ValuesController(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("listarTodos")]
        public async Task<ActionResult<List<CardapioCategoria>>> ListarTodos()
        {
            var resultado = await _repositorio.ObterTodasEntidades<CardapioCategoria>();

            return resultado;
        }

        [HttpGet("listarpor/{codigo:int}")]
        public async Task<ActionResult<CardapioCategoria>> ListarTodos(int codigo)
        {
            var resultado = await _repositorio.ObterEntidadePorChavePrimaria<CardapioCategoria>(codigo);

            return resultado;
        }
    }
}
