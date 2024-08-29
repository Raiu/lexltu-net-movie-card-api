using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationExtensions {

    public static async Task SeedDataAsync (this IApplicationBuilder builder) {
        using var scope = builder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<ApiDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        try {
            await SeedData.InitAsync(context);
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}