namespace CRUDAccountDemo.API.Extensions;

using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateApiInfo(description));
        }
    }

    private static OpenApiInfo CreateApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "CRUDAccountDemo API",
            Version = description.ApiVersion.ToString(),
            Description = "A simple account management API for learning purposes."
        };

        if (description.IsDeprecated)
            info.Description += " This version is deprecated.";

        return info;
    }
}
