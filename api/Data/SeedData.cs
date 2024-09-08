using Api.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class SeedData
{
    private static readonly Faker s_faker = new("sv");

    private static readonly Random s_rnd = new();

    public static async Task InitAsync(ApiDbContext db)
    {
        if (await db.Movies.AnyAsync())
            return;

        try
        {
            await db.Actors.AddRangeAsync(GenerateActors(500));
            await db.Directors.AddRangeAsync(GenerateDirectors(50));
            await db.Genres.AddRangeAsync(GenerateGenres(20));
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        var actors = await db.Actors.ToListAsync();
        var directors = await db.Directors.ToListAsync();
        var genres = await db.Genres.ToListAsync();

        try
        {
            var movies = GenerateMovies(actors, directors, genres, 200);
            await db.Movies.AddRangeAsync(movies);
            await db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private static List<Movie> GenerateMovies(
        List<Actor> actors,
        List<Director> directors,
        List<Genre> genres,
        int numberOf
    )
    {
        var movies = new List<Movie>();

        for (var i = 0; i < numberOf; i++)
        {
            var selectedActors = actors
                .OrderBy(x => s_rnd.Next())
                .Take(s_rnd.Next(1, Math.Min(actors.Count, 11)))
                .ToList();

            var selectedGenres = genres
                .OrderBy(x => s_rnd.Next())
                .Take(s_rnd.Next(1, Math.Min(genres.Count, 3)))
                .ToList();

            var movie = new Faker<Movie>("sv");
            movie.RuleFor(m => m.Title, f => f.Company.CatchPhrase());
            movie.RuleFor(
                m => m.Description,
                f => string.Join(" ", f.Lorem.Words(f.Random.Number(10, 30)))
            );
            movie.RuleFor(
                m => m.ReleaseDate,
                f => f.Date.FutureDateOnly(5, DateOnly.FromDateTime(DateTime.Today))
            );
            movie.RuleFor(m => m.Rating, f => f.Random.Byte(max: 10));
            movie.RuleFor(m => m.Director, f => f.PickRandom(directors));
            movie.RuleFor(m => m.Actors, _ => selectedActors);
            movie.RuleFor(m => m.Genres, _ => selectedGenres);
            movies.Add(movie.Generate());
        }
        return movies;
    }

    private static List<Actor> GenerateActors(int numberOf) =>
        Enumerable
            .Range(1, numberOf)
            .Select(x =>
                new Faker<Actor>("sv")
                    .RuleFor(a => a.Name, f => f.Name.FullName())
                    .RuleFor(
                        a => a.DateOfBirth,
                        f => DateOnly.FromDateTime(f.Date.Past(80, DateTime.Now.AddYears(-18)))
                    )
                    .Generate()
            )
            .ToList();

    private static List<Director> GenerateDirectors(int numberOf) =>
        Enumerable
            .Range(1, numberOf)
            .Select(x =>
                new Faker<Director>("sv")
                    .RuleFor(d => d.Name, f => f.Person.FullName)
                    .RuleFor(
                        d => d.DateOfBirth,
                        f => DateOnly.FromDateTime(f.Date.Past(80, DateTime.Now.AddYears(-40)))
                    )
                    .RuleFor(
                        d => d.ContactInformation,
                        f =>
                            new Faker<ContactInformation>("sv")
                                .RuleFor(ci => ci.Email, _ => f.Person.Email)
                                .RuleFor(ci => ci.PhoneNumber, _ => f.Person.Phone)
                                .Generate()
                    )
                    .Generate()
            )
            .ToList();

    private static List<Genre> GenerateGenres(int numberOf) =>
        Enumerable
            .Range(1, numberOf)
            .Select(x =>
                new Faker<Genre>("sv").RuleFor(g => g.Name, f => f.Lorem.Word()).Generate()
            )
            .ToList();
}
