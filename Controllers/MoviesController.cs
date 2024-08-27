using System.Data;
using Api.Data;
using Api.Extensions;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly IMapper _map;

    public MoviesController(ApiDbContext context, IMapper mapper)
    {
        _db = context;
        _map = mapper;
    }

    /* Create
     ***********/
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(MovieReqDto dto)
    {
        var movie = _map.Map<Movie>(dto);
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetMovieDetails", new { id = movie.Id }, movie);
    }

    /* Read all
     ***********/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieResDto>>> ReadMovies()
    {
        return await _map.ProjectTo<MovieResDto>(_db.Movies).ToListAsync();
    }

    /* Read by Id
     *************/
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieResDto>> ReadMovie(int id)
    {
        var dto = await _map.ProjectTo<MovieResDto>(_db.Movies)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }

    /* Read by Id with Details
     **************************/
    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieResDetailsDto>> ReadMovieDetails(int id)
    {
        var dto = await _map.ProjectTo<MovieResDetailsDto>(_db.Movies)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }

    /* Update
     ***********/
    [HttpPut("{id}")]
    public async Task<ActionResult<MovieResDetailsDto>> UpdateMovie(int id, MovieUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var current = await _map.ProjectTo<Movie>(_db.Movies)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (current is null)
            return NotFound();

        _map.Map(dto, current);

        if (dto.DirectorId != 0)
        {
            var director = await _db.Directors.FindAsync(dto.DirectorId);
            if (director == null)
                return BadRequest();
            current.Director = director;
        }

        if (dto.ActorsId != null && dto.ActorsId.Count != 0)
        {
            var actors = await _db.Actors.Where(x => dto.ActorsId.Contains(x.Id)).ToListAsync();
            current.Actors = actors;
        }

        if (dto.GenresId != null && dto.GenresId.Count != 0)
        {
            var genres = await _db.Genres.Where(x => dto.GenresId.Contains(x.Id)).ToListAsync();
            current.Genres = genres;
        }

        try
        {
            _db.Entry(current).Property(x => x.DirectorId).IsModified = false;
            await _db.SaveChangesAsync();
        }
        catch (DBConcurrencyException err)
        {
            Console.WriteLine(err);
            throw;
        }

        return Ok(_map.Map<MovieResDetailsDto>(current));
    }

    /* Delete
     ***********/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMovie(int id)
    {
        var movie = await _db.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
