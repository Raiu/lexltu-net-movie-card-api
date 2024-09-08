using Api.Models;
using AutoMapper;

namespace Api.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, Movie>().ReverseMap();
        CreateMap<Movie, MovieResDto>();
        CreateMap<Movie, MovieResDetailsDto>()
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
        CreateMap<MovieReqDto, Movie>()
            .ForMember(dest => dest.DirectorId, opt => opt.MapFrom(src => src.DirectorId))
            .ForMember(
                dest => dest.Actors,
                opt => opt.MapFrom(src => src.ActorsId.Select(id => new Actor { Id = id }).ToList())
            )
            .ForMember(
                dest => dest.Genres,
                opt => opt.MapFrom(src => src.GenresId.Select(id => new Genre { Id = id }).ToList())
            );
        CreateMap<MovieUpdateDto, Movie>();

        CreateMap<Director, DirectorDto>().ReverseMap();
        CreateMap<Director, DirectorDto>().ReverseMap();
        CreateMap<DirectorCreateDto, Director>();

        CreateMap<Actor, ActorResponseDto>().ReverseMap();
        CreateMap<Actor, ActorDto>().ReverseMap();
        CreateMap<ActorCreateDto, Actor>();

        CreateMap<Genre, GenreResponseDto>().ReverseMap();
    }
}
