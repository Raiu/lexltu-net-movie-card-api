using Api.Data;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ActorService(ApiDbContext context, IMapper mapper) : IActorService
{
    private readonly ApiDbContext _db = context;

    private readonly IMapper _am = mapper;

    /// <summary>
    /// This asynchronous method creates a new actor using the provided data transfer object.
    /// </summary>
    /// <param name="ActorCreateDto">ActorCreateDto is a data transfer object (DTO) that contains the
    /// information needed to create a new actor. It likely includes properties such as the actor's
    /// name, date of birth, biography, and any other relevant details required for creating a new actor
    /// in the system.</param>
    public async Task<ActorDto> CreateAsync(ActorCreateDto dto)
    {
        var actor = _am.Map<Actor>(dto);
        _db.Actors.Add(actor);
        await _db.SaveChangesAsync();
        return _am.Map<ActorDto>(actor);
    }

    /// <summary>
    /// This C# function asynchronously retrieves all actors and returns them as a collection of
    /// ActorDto objects.
    /// </summary>
    public async Task<IEnumerable<ActorDto>> GetAllAsync() => await GetAllAsync<ActorDto>();

    /// <summary>
    /// This C# function asynchronously retrieves all items of type T.
    /// </summary>
    /// <summary>
    /// This C# function asynchronously retrieves all items of type T.
    /// </summary>
    public async Task<IEnumerable<T>> GetAllAsync<T>() =>
        await _am.ProjectTo<T>(_db.Actors).ToListAsync();

    /// <summary>
    /// This C# function asynchronously retrieves an ActorDto object by its ID.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// an actor.</param>
    public async Task<ActorDto> GetByIdAsync(int id) =>
        await _am.ProjectTo<ActorDto>(_db.Actors.Where(x => x.Id == id)).FirstAsync();

    /// <summary>
    /// This C# function asynchronously updates an actor with the provided ID using the data in the
    /// ActorDto object.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// the actor that you want to update in the database.</param>
    /// <param name="ActorDto">ActorDto is a data transfer object (DTO) that represents an actor in your
    /// application. It likely contains properties such as Id, Name, Age, Gender, etc., to hold
    /// information about an actor.</param>
    public async Task<ActorDto?> UpdateAsync(int id, ActorDto dto)
    {
        var current = await _am.ProjectTo<Actor>(_db.Actors.Where(x => x.Id == id))
            .FirstOrDefaultAsync();

        if (current == null)
            return null;

        _am.Map(current, dto);

        await _db.SaveChangesAsync();

        return _am.Map<ActorDto>(current);
    }

    /// <summary>
    /// This C# function is an asynchronous method that deletes a record based on the provided ID.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// the item that you want to delete asynchronously.</param>
    public async Task<bool> DeleteAsync(int id)
    {
        var actor = await _db.Actors.FindAsync(id);
        if (actor == null)
            return false;

        _db.Actors.Remove(actor);
        await _db.SaveChangesAsync();

        return true;
    }
}
