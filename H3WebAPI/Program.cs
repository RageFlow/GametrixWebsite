using H3WebAPI.Models;
using H3WebAPI.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using H3WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(config =>
{
    config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<RiotService>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ConfigurationService>();

builder.Configuration.AddJsonFile("SwaggerDocs.json", true);
var swaggerDocs = new SwaggerDocDefinitionOptions();
builder.Configuration.GetSection(SwaggerDocDefinitionOptions.SwaggerDocs).Bind(swaggerDocs);
builder.Services.AddSingleton(swaggerDocs);

builder.Services.AddSwaggerGen(e =>
{
    foreach (var definition in swaggerDocs.Definitions)
    {
        e.SwaggerDoc(definition.Name, new OpenApiInfo
        {
            Title = definition.Title,
            Version = definition.Version,
            Description = definition.Description
        });
    }

    e.DocInclusionPredicate((docName, apiDesc) =>
    {
        var versions = apiDesc.ActionDescriptor.EndpointMetadata
            .OfType<ApiVersionAttribute>()
            .ToList();

        var validDocNames = swaggerDocs.Definitions.Select(x => x.Name).ToArray();
        return validDocNames.Contains(docName) && versions.Any(v => $"{v.Product}".Equals(docName, StringComparison.OrdinalIgnoreCase));
    });

    e.DocumentFilter<AddSwaggerDocumentFilter>();

    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");

    foreach (var xmlFile in xmlFiles)
    {
        e.IncludeXmlComments(xmlFile, true);
        e.UseInlineDefinitionsForEnums();
    }

    e.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "XApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { key, new List<string>() }
    };
    e.AddSecurityRequirement(requirement);

});

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseSwagger();

var docDefs = app.Services.GetService<SwaggerDocDefinitionOptions>();
app.UseSwaggerUI(c =>
{
    if (docDefs != null)
    {
        foreach (var definition in docDefs.Definitions)
        {
            c.SwaggerEndpoint(definition.EndPoint, definition.Title);
        }
    }
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
});

app.UseMiddleware<APIKeyMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
