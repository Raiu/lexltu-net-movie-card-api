using Api.Data;
using Api.Exceptions;
using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class MovieService(
    ApiDbContext dbContext,
    IMapper autoMapper,
    ActionContext actionContext
) : IMovieService
{
    private readonly ApiDbContext _db = dbContext;

    private readonly IMapper _am = autoMapper;

    private readonly ActionContext _ac = actionContext;

    public async Task<Movie> CreateAsync(MovieCreateDto dto)
    {
        var movie = _am.Map<Movie>(dto);
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return movie;
    }

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

    public async Task<MovieDto> GetByIdAsync(int id) => await GetByIdAsync<MovieDto>(id);

    public async Task<T> GetByIdAsync<T>(int id)
        where T : IMovieId =>
        await _am.ProjectTo<T>(_db.Movies).Where(x => x.Id == id).FirstAsync();

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

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie == null)
            return false;

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddActorsToMovieAsync(int id, ICollection<ActorDto> dtos)
    {
        var movie = await _db.Movies.FindAsync(id);

        if (movie == null)
            return false;

        foreach (var dto in dtos)
        {
            var actor = await _db.Actors.FindAsync(dto.Id);
            if (actor == null)
            {
                actor = _am.Map<Actor>(dto);
                _db.Actors.Add(actor);
                await _db.SaveChangesAsync();
            }
            movie.Actors.Add(actor);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<int> PartialAsync(int id, JsonPatchDocument patchDocument)
    {
        var movie =
            await _db.Movies.FindAsync(id)
            ?? throw new NotFoundException($"Movie with id: {id} was not found");
        patchDocument.ApplyTo(movie);

        if (!_ac.ModelState.IsValid)
            throw new BadRequestException(
                _ac.ModelState.ToString() ?? "Model is not valid"
            );

        return await _db.SaveChangesAsync();
    }
}
