using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController(IDirectorService directorService) : ControllerBase
{
    private readonly IDirectorService _ds = directorService;

    /* Create
     ***********/
    [HttpPost]
    public async Task<ActionResult<DirectorDto>> CreateDirector(DirectorCreateDto dto)
    {
        var director = await _ds.CreateAsync(dto);
        return CreatedAtAction(nameof(ReadDirector), new { id = director.Id }, director);
    }

    /* Read all
     ***********/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DirectorDto>>> ReadDirectors() =>
        Ok(await _ds.GetAllAsync());

    /* Read by Id
     *************/
    [HttpGet("{id}")]
    public async Task<ActionResult<DirectorDto>> ReadDirector(int id) =>
        Ok(await _ds.GetByIdAsync(id));

    /* Update
     ***********/
    [HttpPut("{id}")]
    public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, DirectorDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var updatedDto = await _ds.UpdateAsync(id, dto);

        if (updatedDto == null)
            return NotFound();

        return Ok(updatedDto);
    }

    /* Delete
     ***********/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDirector(int id)
    {
        var result = await _ds.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
