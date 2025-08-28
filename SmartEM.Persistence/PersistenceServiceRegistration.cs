using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartEM.Application.Contracts.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace SmartEM.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SmartEMDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
