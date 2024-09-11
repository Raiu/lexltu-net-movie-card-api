using Api.Data;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class DirectorService(ApiDbContext context, IMapper mapper) : IDirectorService
{
    private readonly ApiDbContext _db = context;

    private readonly IMapper _am = mapper;

    /// <summary>
    /// The CreateAsync method creates a new DirectorDto asynchronously based on the provided
    /// DirectorCreateDto.
    /// </summary>
    /// <param name="DirectorCreateDto">DirectorCreateDto is a data transfer object (DTO) that contains
    /// the information needed to create a new director. It likely includes properties such as name,
    /// date of birth, nationality, and any other relevant details about the director.</param>
    public async Task<DirectorDto> CreateAsync(DirectorCreateDto dto)
    {
        var director = _am.Map<Director>(dto);
        _db.Directors.Add(director);
        await _db.SaveChangesAsync();
        return _am.Map<DirectorDto>(director);
    }

    /// <summary>
    /// This C# function asynchronously retrieves all director data and returns it as a collection of
    /// DirectorDto objects.
    /// </summary>
    public async Task<IEnumerable<DirectorDto>> GetAllAsync() => await GetAllAsync<DirectorDto>();

    /// <summary>
    /// This C# function asynchronously retrieves all items of type T.
    /// </summary>
    public async Task<IEnumerable<T>> GetAllAsync<T>() =>
        await _am.ProjectTo<T>(_db.Directors).ToListAsync();

    /// <summary>
    /// This C# function asynchronously retrieves a DirectorDto object by its ID.
    /// </summary>
    /// <param name="id">The `id` parameter in the `GetByIdAsync` method is an integer value that
    /// represents the unique identifier of the director whose information you want to retrieve
    /// asynchronously.</param>
    public async Task<DirectorDto> GetByIdAsync(int id) =>
        await _am.ProjectTo<DirectorDto>(_db.Directors.Where(x => x.Id == id)).FirstAsync();

    /// <summary>
    /// This C# function asynchronously updates a DirectorDto object with the specified ID.
    /// </summary>
    /// <param name="id">The `id` parameter is an integer value that represents the unique identifier of
    /// the director that you want to update.</param>
    /// <param name="DirectorDto">The `DirectorDto` is a data transfer object (DTO) that represents a
    /// director entity. It likely contains properties such as `Id`, `Name`, `BirthDate`, `Nationality`,
    /// etc., to hold information about a director. In the context of the `UpdateAsync` method you
    /// provided</param>
    public async Task<DirectorDto?> UpdateAsync(int id, DirectorDto dto)
    {
        var current = await _am.ProjectTo<Director>(_db.Directors.Where(x => x.Id == id))
            .FirstOrDefaultAsync();

        if (current == null)
            return null;

        _am.Map(current, dto);

        await _db.SaveChangesAsync();

        return _am.Map<DirectorDto>(current);
    }

    /// <summary>
    /// This C# function is an asynchronous method that deletes a record based on the provided ID.
    /// </summary>
    /// <param name="id">The `id` parameter in the `DeleteAsync` method is an integer value that
    /// represents the unique identifier of the item that you want to delete asynchronously.</param>
    public async Task<bool> DeleteAsync(int id)
    {
        var director = await _db.Directors.FindAsync(id);
        if (director == null)
            return false;

        _db.Directors.Remove(director);
        await _db.SaveChangesAsync();

        return true;
    }
}
