using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace H3WebAPI.Models
{
    public sealed class AddSwaggerDocumentFilter : IDocumentFilter
    {
        private static Dictionary<string, string> coreEndpointAPIMPrefixLookup = new Dictionary<string, string>
        {
            { "internal", "internal"},
            { "external", "external"}
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            string url = string.Empty;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Development")
            {
                url = "https://localhost:5001";
                swaggerDoc.Servers.Add(new OpenApiServer { Url = url });
            }
            if (environment == "Staging")
            {
                url = "https://website.dk:5002";
                swaggerDoc.Servers.Add(new OpenApiServer { Url = url });
            }
            if (environment == "Production")
            {
                //url = "https://website.dk:5003";
                url = "https://website.dk:5001";
                swaggerDoc.Servers.Add(new OpenApiServer { Url = url });
            }

            url = $"https://website.dk:{Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT")}";

            swaggerDoc.Servers.Add(new OpenApiServer { Url = url });

#if !DEBUG
            coreEndpointAPIMPrefixLookup.TryGetValue(context.DocumentName, out string endpointValue);

            OpenApiPaths paths = new OpenApiPaths();

            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(!string.IsNullOrWhiteSpace(endpointValue) ? $"/{endpointValue}{path.Key}" : path.Key, path.Value);
            }

            swaggerDoc.Paths = paths;
#endif
        }
    }
}
