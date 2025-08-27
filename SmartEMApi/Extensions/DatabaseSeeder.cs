using MediatR;
using SmartEM.Application.Features.Users.Commands.SeedSuperAdmin;
using SmartEM.Application.Utils;

namespace SmartEMApi.Extensions;

internal class DatabaseSeeder()
{
    public static async Task SeedDatabaseAsync(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var databaseSeeder = scopedServices.GetRequiredService<DatabaseSeeder>();
            await databaseSeeder.SeedSuperAdmin(scopedServices);
        }
    }
    private async Task SeedSuperAdmin(IServiceProvider serviceProvider)
    {
        var seedSuperadminHandler = serviceProvider.GetRequiredService<IRequestHandler<SeedSuperAdminCommand, OperationResponse>>();
        await seedSuperadminHandler.Handle(new SeedSuperAdminCommand(), CancellationToken.None);
    }
}
