using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
// ReSharper disable PartialTypeWithSinglePart

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Run();

namespace Estudos.SSE.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
    }
}