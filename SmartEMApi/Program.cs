using Microsoft.EntityFrameworkCore;
using SmartEM.Application.Utils;
using SmartEMApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable(ApplicationConstants.CONNECTION_STRING)
        ?? builder.Configuration.GetValue<string>(ApplicationConstants.CONNECTION_STRING);


builder.ConfigureSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddControllers()
    .AddCustomJsonOptions()
    .AddApiBehaviorConfiguration();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
var app = builder.ConfigureServices(connectionString!);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
if (!app.Environment.IsEnvironment(ApplicationConstants.TEST_ENVIRONMENT))
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Migrate();

await DatabaseSeeder.SeedDatabaseAsync(app);

app.Run();