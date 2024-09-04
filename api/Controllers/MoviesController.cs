using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public record ReadMoviesParameters
{
    public MovieSearchParameters? Search { get; init; }
    public MovieSortingParameters? Sorting { get; init; }
}

public record MovieSearchParameters
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? ActorName { get; init; }
    public string? DirectorName { get; init; }
    public DateOnly? ReleaseDate { get; init; }
}

public record MovieSortingParameters
{
    public IEnumerable<MovieSorting>? MovieSortings { get; init; }
}

public record MovieSorting
{
    public MovieSortingField Field { get; init; }
    public MovieSortingOrder? Order { get; init; }
}

public enum MovieSortingField
{
    Title,
    ReleaseDate,
    Rating,
}

public enum MovieSortingOrder
{
    Ascending,
    Descending,
    Newest,
    Oldest,
    Lowest,
    Highest,
}

[Route("api/[controller]")]
[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    private readonly IMovieService _ms = movieService;

    /* Create
     ***********/
    [HttpPost]
    public async Task<ActionResult<MovieResDetailsDto>> CreateMovie(MovieCreateDto dto)
    {
        var movie = await _ms.CreateAsync(dto);
        return CreatedAtAction(nameof(ReadMovieDetails), new { id = movie.Id }, movie);
    }

    /* Read all
     ***********/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieResDto>>> ReadMovies(
        [FromQuery] ReadMoviesParameters? parameters,
        [FromQuery] bool IncludeDirector = true,
        [FromQuery] bool IncludeActors = true,
        [FromQuery] bool IncludeGenres = true
    )
    {
        return Ok(
            await _ms.GetAllAsync(
                parameters,
                IncludeActors,
                IncludeDirector,
                IncludeGenres
            )
        );
    }

    /* Read by Id
     *************/
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieResDto>> ReadMovie(int id)
    {
        var dto = await _ms.GetByIdAsync<MovieResDto>(id);

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }

    /* Read by Id with Details
     **************************/
    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieResDetailsDto>> ReadMovieDetails(int id)
    {
        var dto = await _ms.GetByIdAsync<MovieResDetailsDto>(id);

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }

    /* Update
     ***********/
    [HttpPut("{id}")]
    public async Task<ActionResult<MovieResDetailsDto>> UpdateMovie(
        int id,
        MovieUpdateDto dto
    )
    {
        if (id != dto.Id)
            return BadRequest();

        var updatedDto = await _ms.UpdateAsync(id, dto);
        if (updatedDto == null)
            return NotFound();

        return Ok(updatedDto);
    }

    /* Delete
     ***********/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMovie(int id)
    {
        var result = await _ms.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
