using Estudos.CleanArchitecture.Modular.API.Configurations.Swagger;
using Estudos.CleanArchitecture.Modular.API.Presenters;
using Estudos.CleanArchitecture.Modular.CrossCutting.IOC.Bootstrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
// ReSharper disable PartialTypeWithSinglePart

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerConfiguration();

builder.Services.InitializeApplication();
builder.Services.AddPresenters();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Estudos.CleanArchitecture.Modular.API.Program).Assembly));

var app = builder.Build();

app.UseSwaggerConfiguration(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();

namespace Estudos.CleanArchitecture.Modular.API
{
    public partial class Program
    {
    }
}