using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Estudos.CleanArchitecture.Modular.API.Configurations.Swagger
{
    [ExcludeFromCodeCoverage]
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private const string ApiName = "Estudos.CleanArchitecture.Modular";
    
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var versionDescription in _provider.ApiVersionDescriptions)
            {
                var description = new StringBuilder("API com as funcionalidades de bureau");

                if (versionDescription.ApiVersion.ToString().Equals("1.0"))
                {
                    description.Append("<br/><br/>Versão inicial.");
                }

                if (versionDescription.IsDeprecated)
                {
                    description.Append("<br/><br/><span style=\"color: #ff0000;font-weight: bold;\">Esta versão está obsoleta.</span>");
                }

                const string repositoryUrl = "NONE";

                var info = new OpenApiInfo
                {
                    Title = ApiName,
                    Version = versionDescription.ApiVersion.ToString(),
                    Description = description.ToString(),
                    Contact = new OpenApiContact
                    {
                        Name = "Detecção de Fraudes",
                        Email = "None",
                        Url = new Uri(repositoryUrl)
                    }
                };

                options.SwaggerDoc(versionDescription.GroupName, info);
                // TODO AJUSTAR AQUI
                // options.DocumentFilter<HealthCheckDocumentFilter>();
            }
        }
    }
}