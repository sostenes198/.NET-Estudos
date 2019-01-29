using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OpenTracing.Tag;

namespace Payment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ITracer tracer;

    public ValuesController(ITracer tracer)
    {
        this.tracer = tracer;
    }

    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        using (var scope = StartServerSpan(tracer, "Processing"))
        {
            Thread.Sleep(2000);
            return new[] { "Payment done!" };
        }
    }

    public static IScope StartServerSpan(ITracer tracer, string operationName)
    {
        var spanBuilder = tracer
           .BuildSpan(operationName)
           .WithTag(Tags.SpanKind, Tags.SpanKindServer)
           .StartActive(true);;

        return spanBuilder;
    }
}