using System.ComponentModel.DataAnnotations;
using Api.Interfaces;

namespace Api.Models;

#nullable disable

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public required string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }

    [Range(0, 10)]
    public byte Rating { get; set; }

    // Director
    public int DirectorId { get; set; }

    public Director Director { get; set; }

    // Actor
    public ICollection<Actor> Actors { get; set; }

    // Genre
    public ICollection<Genre> Genres { get; set; }
}

public record MovieDto : IDtoId
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateOnly ReleaseDate { get; set; }
}

public record MovieResDto : IDtoId
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateOnly ReleaseDate { get; set; }
}

public record MovieResDetailsDto : MovieResDto
{
    public DirectorDto Director { get; set; }

    public List<ActorResponseDto> Actors { get; set; }

    public List<GenreResponseDto> Genres { get; set; }
}

public record MovieReqDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Range(0, 10)]
    public byte Rating { get; set; }

    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }

    public int DirectorId { get; set; }

    public List<int> ActorsId { get; set; }

    public List<int> GenresId { get; set; }
}

public record MovieCreateDto : IDtoId
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public required string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }

    [Range(0, 10)]
    public byte Rating { get; set; }

    public DirectorDto Director { get; set; }

    // Actor
    public ICollection<ActorDto> Actors { get; set; }

    // Genre
    public ICollection<GenreDto> Genres { get; set; }
}

public record MovieUpdateDto : IDtoId
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(2000)]
    public string Description { get; set; }

    [Range(0, 10)]
    public byte Rating { get; set; }

    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }

    public int DirectorId { get; set; }

    public List<int> ActorsId { get; set; }

    public List<int> GenresId { get; set; }
}

/*
public DirectorResponseDto Director { get; set; }

public List<ActorResponseDto> Actors { get; set; }

public List<GenreResponseDto> Genres { get; set; }
 */
