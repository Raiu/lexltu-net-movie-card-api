using Microsoft.AspNetCore.JsonPatch;

namespace Api.Services;

public interface IServiceBase<TEntity, TDto>
    where TEntity : class
{
    Task<TDto> CreateAsync<TCreateDto>(TCreateDto dto);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync<T>();
    Task<TDto> GetByIdAsync(int id);
    Task<T> GetByIdAsync<T>(int id);
    Task<TDto> UpdateAsync<TUpdateDto>(int id, TUpdateDto dto);
    Task<TReturn> UpdateAsync<TReturn, TUpdateDto>(int id, TUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TDto> PartialAsync(int id, JsonPatchDocument patchDocument);
    Task<T> PartialAsync<T>(int id, JsonPatchDocument patchDocument);
}
