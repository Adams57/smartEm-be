using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartEM.Application;
using SmartEM.Persistence;

namespace SmartEMApi.Extensions;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddPersistenceServices(connectionString);
        builder.Services.AddApplicationServices(builder.Configuration);
        //builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
        return builder.Build();
    }

    public static void ConfigureSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartEM API", Version = "v1" });
            var security = new Dictionary<string, IEnumerable<string>>
            {
                            {"Bearer", Array.Empty<string>()},
            };

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
            c.SchemaFilter<GuidEmptySchemaFilter>();
        });
    }

    public static WebApplication Migrate(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SmartEMDbContext>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartEMDbContext>();
            try
            {
                dbContext?.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Database migration or connection failed: {ex.Message}");
                throw;
            }
        }
        return app;
    }
}
