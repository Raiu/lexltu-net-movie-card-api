using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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

    [HttpPatch("{id}")]
    public async Task<ActionResult<MovieDto>> PatchMovie(
        int id,
        [FromBody] JsonPatchDocument patchDocument
    )
    {
        if (patchDocument == null)
            return BadRequest(ModelState);

        var movie = await _ms.GetByIdAsync<Movie>(id);

        if (movie == null)
            return NotFound();

        patchDocument.ApplyTo(movie);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _ms.UpdateAsync(id, movie);
    }

    [HttpPost("{id}/actors")]
    public async Task<ActionResult<MovieDto>> AddActorToMovie(
        int id,
        ICollection<ActorDto> dtos
    )
    {
        var result = await _ms.AddActorsToMovieAsync(id, dtos);
        if (!result)
            return NotFound();

        return CreatedAtAction(nameof(ReadMovieDetails), new { id = id });
    }
}
