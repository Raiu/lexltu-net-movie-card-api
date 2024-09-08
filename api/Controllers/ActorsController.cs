using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController(IActorService actorService) : ControllerBase
{
    private readonly IActorService _as = actorService;

    /* Create
     ***********/
    [HttpPost]
    public async Task<ActionResult<ActorDto>> CreateActor(ActorCreateDto dto)
    {
        var actor = await _as.CreateAsync(dto);
        return CreatedAtAction(nameof(ReadActor), new { id = actor.Id }, actor);
    }

    /* Read all
     ***********/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> ReadActors() =>
        Ok(await _as.GetAllAsync());

    /* Read by Id
     *************/
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> ReadActor(int id) => Ok(await _as.GetByIdAsync(id));

    /* Update
     ***********/
    [HttpPut("{id}")]
    public async Task<ActionResult<ActorDto>> UpdateActor(int id, ActorDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var updatedDto = await _as.UpdateAsync(id, dto);

        if (updatedDto == null)
            return NotFound();

        return Ok(updatedDto);
    }

    /* Delete
     ***********/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActor(int id)
    {
        var result = await _as.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
