using System.ComponentModel.DataAnnotations;

namespace Api.Models;

#nullable disable

public class Actor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    // Movie
    public ICollection<Movie> Movies { get; set; }
}

public class ActorDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
}

public class ActorCreateDto
{
    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
}

public class ActorResponseDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
}
