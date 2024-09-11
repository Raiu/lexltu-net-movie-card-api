using Api.Models;

namespace Api.Interfaces;

public interface IDirectorService
{
    Task<DirectorDto> CreateAsync(DirectorCreateDto dto);

    Task<IEnumerable<DirectorDto>> GetAllAsync();

    Task<IEnumerable<T>> GetAllAsync<T>();

    Task<DirectorDto> GetByIdAsync(int id);

    Task<DirectorDto?> UpdateAsync(int id, DirectorDto dto);

    Task<bool> DeleteAsync(int id);
}
