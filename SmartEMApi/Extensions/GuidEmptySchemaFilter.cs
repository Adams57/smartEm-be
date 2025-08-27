using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartEMApi.Extensions;

public class GuidEmptySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {

        if (context.Type == typeof(Guid))
        {
            schema.Default = new Microsoft.OpenApi.Any.OpenApiString(Guid.Empty.ToString());
        }
    }
}
