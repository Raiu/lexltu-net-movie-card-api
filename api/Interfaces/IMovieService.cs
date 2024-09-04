using Api.Controllers;
using Api.Models;

namespace Api.Interfaces;

public interface IMovieService
{
    Task<Movie> CreateAsync(MovieCreateDto dto);

    Task<IEnumerable<MovieResDto>> GetAllAsync(
        ReadMoviesParameters? parameters,
        bool IncludeDirector,
        bool IncludeActors,
        bool IncludeGenres
    );

    Task<MovieDto> GetByIdAsync(int id);

    Task<T> GetByIdAsync<T>(int id)
        where T : IDtoId;

    Task<MovieResDetailsDto?> UpdateAsync(int id, MovieUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}
