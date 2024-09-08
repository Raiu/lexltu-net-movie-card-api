using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public override int SaveChanges()
    {
        CheckDuplicateActor();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        CheckDuplicateActor();
        return await base.SaveChangesAsync(token);
    }

    private void CheckDuplicateActor()
    {
        var entities = ChangeTracker
            .Entries<Actor>()
            .Where(a => a.State == EntityState.Added || a.State == EntityState.Modified)
            .Select(e => e.Entity);
        foreach (var entity in entities)
        {
            if (Actors.Any(a => a.Name == entity.Name && a.DateOfBirth == entity.DateOfBirth))
            {
                throw new DbUpdateException("Duplicate Actor");
            }
        }
    }
}
