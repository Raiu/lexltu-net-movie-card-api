using Api.Models;
using Api.Services;

namespace Api.Interfaces;

public interface IMovieService : IServiceBase<Movie, MovieDto>
{
    Task<IEnumerable<MovieResDto>> GetAllAsync(
        ReadMoviesParameters? parameters,
        bool IncludeDirector,
        bool IncludeActors,
        bool IncludeGenres
    );

    Task<bool> AddActorsToMovieAsync(int id, ICollection<ActorDto> dtos);
}

/* Task<Movie> CreateAsync(MovieCreateDto dto);

    Task<IEnumerable<MovieResDto>> GetAllAsync(
        ReadMoviesParameters? parameters,
        bool IncludeDirector,
        bool IncludeActors,
        bool IncludeGenres
    );

    Task<MovieDto> GetByIdAsync(int id);

    Task<T> GetByIdAsync<T>(int id)
        where T : IMovieId;

    Task<MovieResDetailsDto?> UpdateAsync(int id, MovieUpdateDto dto);

    Task<bool> DeleteAsync(int id);

    Task<bool> AddActorsToMovieAsync(int id, ICollection<ActorDto> dtos);

    Task<T> PartialAsync<T>(int id, JsonPatchDocument patchDocument); */
