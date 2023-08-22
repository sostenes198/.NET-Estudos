using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Estudos.CleanArchitecture.Modular.API.Configurations.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] {api.GroupName};
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] {controllerActionDescriptor.ControllerName};
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                options.DocInclusionPredicate((_, _) => true);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((document, request) =>
                {
                    var url = Microsoft.AspNetCore.Http.Extensions.UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path, request.QueryString);
                    string basePath = (request.Headers.TryGetValue("X-Original-URI", out var header) ? header.FirstOrDefault() : url)!;
                    basePath = basePath.Split(new[] {"/swagger"}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()!;

                    document.Servers = string.IsNullOrEmpty(basePath) ? new List<OpenApiServer> {new() {Url = $"{request.Scheme}://{request.Host.Value}"}} : new List<OpenApiServer> {new() {Url = basePath}};
                });
            });

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var notice = description.IsDeprecated ? " This API version has been deprecated." : string.Empty;

                    c.SwaggerEndpoint($"./{description.GroupName}/swagger.json",
                                      $"V{description.ApiVersion} - Bureau API" + notice);
                }

                c.DocExpansion(DocExpansion.List);
            });

            return app;
        }
    }
}