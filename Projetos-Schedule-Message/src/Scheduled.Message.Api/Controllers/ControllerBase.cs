using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Scheduled.Message.Api.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ControllerBase: Microsoft.AspNetCore.Mvc.ControllerBase
{
    
}