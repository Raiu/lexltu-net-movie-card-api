using Api.Controllers;
using Api.Data;
using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

/* The `MovieService` class in C# provides methods for asynchronously creating, retrieving, updating,
and deleting movie entities using data transfer objects and entity mapping. */
public class MovieService(ApiDbContext context, IMapper mapper) : IMovieService
{
    private readonly ApiDbContext _db = context;

    private readonly IMapper _am = mapper;

    /// <summary>
    /// This C# function asynchronously creates a new movie based on the provided data transfer object
    /// (DTO).
    /// </summary>
    /// <param name="MovieCreateDto">The `MovieCreateDto` is a data transfer object (DTO) that contains
    /// the information needed to create a new `Movie` object. It typically includes properties such as
    /// title, description, release date, genre, and any other relevant details required to create a
    /// movie entity. This DTO is used to</param>
    public async Task<Movie> CreateAsync(MovieCreateDto dto)
    {
        var movie = _am.Map<Movie>(dto);
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return movie;
    }

    /// <summary>
    /// This C# function asynchronously retrieves all movie resources as a collection of MovieResDto
    /// objects.
    /// </summary>
    public async Task<IEnumerable<MovieResDto>> GetAllAsync(
        ReadMoviesParameters? parameters,
        bool IncludeDirector,
        bool IncludeActors,
        bool IncludeGenres
    )
    {
        var query = _db.Movies.AsQueryable();

        query = query.FilterMovieNonGeneric(parameters?.Search);
        query = query.SortMovieNonGeneric(parameters?.Sorting);

        return await _am.ProjectTo<MovieResDto>(query).ToListAsync();
    }

    /// <summary>
    /// This C# function asynchronously retrieves a movie by its ID and returns a MovieDto object.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// a movie.</param>
    public async Task<MovieDto> GetByIdAsync(int id) => await GetByIdAsync<MovieDto>(id);

    /// <summary>
    /// This C# function asynchronously retrieves an object of type T by its ID.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// the item you want to retrieve.</param>
    public async Task<T> GetByIdAsync<T>(int id)
        where T : IDtoId =>
        await _am.ProjectTo<T>(_db.Movies).Where(x => x.Id == id).FirstAsync();

    /// <summary>
    /// This C# function asynchronously updates a movie entity with the provided data.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// the movie that you want to update.</param>
    /// <param name="MovieUpdateDto">The `MovieUpdateDto` is a data transfer object (DTO) that contains
    /// the updated information for a movie. It typically includes properties such as `Title`,
    /// `Description`, `ReleaseDate`, `Genre`, etc. When calling the `UpdateAsync` method, you would
    /// pass an instance of `</param>
    public async Task<MovieResDetailsDto?> UpdateAsync(int id, MovieUpdateDto dto)
    {
        var current = await _am.ProjectTo<Movie>(_db.Movies)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (current == null)
            return null;

        _am.Map(current, dto);

        if (dto.DirectorId != 0)
        {
            var director = await _db.Directors.FindAsync(dto.DirectorId);
            if (director != null)
                current.Director = director;
        }

        if (dto.ActorsId != null && dto.ActorsId.Count != 0)
        {
            var actors = await _db
                .Actors.Where(x => dto.ActorsId.Contains(x.Id))
                .ToListAsync();
            current.Actors = actors;
        }

        if (dto.GenresId != null && dto.GenresId.Count != 0)
        {
            var genres = await _db
                .Genres.Where(x => dto.GenresId.Contains(x.Id))
                .ToListAsync();
            current.Genres = genres;
        }

        await _db.SaveChangesAsync();

        return _am.Map<MovieResDetailsDto>(current);
    }

    /// <summary>
    /// This asynchronous method deletes a record with the specified ID.
    /// </summary>
    /// <param name="id">The `id` parameter in the `DeleteAsync` method is an integer value that
    /// represents the unique identifier of the item that you want to delete asynchronously.</param>
    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie == null)
            return false;

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();

        return true;
    }
}
