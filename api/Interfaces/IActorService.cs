using Api.Models;

namespace Api.Interfaces;

public interface IActorService
{
    Task<ActorDto> CreateAsync(ActorCreateDto dto);

    Task<IEnumerable<ActorDto>> GetAllAsync();

    Task<IEnumerable<T>> GetAllAsync<T>();

    Task<ActorDto> GetByIdAsync(int id);

    Task<ActorDto?> UpdateAsync(int id, ActorDto dto);

    Task<bool> DeleteAsync(int id);
}
