using Api.Models;

namespace Api.Services;

public interface IMovieService : IApiService<Movie, MovieDto>
{
    Task<IEnumerable<MovieResDto>> GetAllAsync(
        ReadMoviesParameters? parameters,
        bool IncludeDirector,
        bool IncludeActors,
        bool IncludeGenres
    );

    Task<bool> AddActorsToMovieAsync(int id, ICollection<ActorDto> dtos);
}
