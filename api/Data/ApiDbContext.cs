using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options) {
    public DbSet<Movie> Movies { get; set; }

    public DbSet<Director> Directors { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Genre> Genres { get; set; }
}
