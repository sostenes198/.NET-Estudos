using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTracing;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ITracer tracer;
    private readonly IHttpClientFactory httpClientFactory;

    public ValuesController(
        ITracer tracer,
        IHttpClientFactory httpClientFactory
    )
    {
        this.tracer = tracer;
        this.httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public IActionResult Test([FromBody] JsonObject obj)
    {
        var body = obj.ToString();
        var context = HttpContext;

        var jBody = JObject.Parse(body);

        JObject customerAccountInfo = new JObject(
            new JProperty("brand", "3"),
            new JProperty("loggedUserTradingAccount", "20060"),
            new JProperty("cpf", "15799560540"));

        string brand = $"{customerAccountInfo["brand"]}";

        string loggedUserTradingAccount = $"{customerAccountInfo["loggedUserTradingAccount"]}";

        string document = $"{customerAccountInfo["cpf"]}";

        string ipGateway = context.Request.Headers["True-Client-IP"];

        string userAgent = context.Request.Headers["User-Agent"];

        string referer = context.Request.Headers["referer"];

        var jNetwork = (JObject) jBody["network"];

        jNetwork.Add(new JProperty("ipGateway", ipGateway));

        jNetwork.Add(new JProperty("userAgent", userAgent));

        jNetwork.Add(new JProperty("referer", referer));

        jBody.Add(new JProperty("brand", brand));

        jBody.Add(new JProperty("loggedUserTradingAccount", loggedUserTradingAccount));

        jBody.Add(new JProperty("document", document));

        jBody["network"] = jNetwork;

        //jBody.Add(new JProperty("network", jNetwork));

        return new OkObjectResult(new {Test = jBody.ToString()});
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {
        var client = httpClientFactory.CreateClient("payment.api");

        using (tracer.BuildSpan("WaitingForValues").StartActive(finishSpanOnDispose: true))
        {
            return JsonConvert.DeserializeObject<List<string>>(
                await client.GetStringAsync("values")
            );
        }
    }
}