using System.ComponentModel.DataAnnotations;

namespace Api.Models;

#nullable disable

public class Director
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    public ContactInformation ContactInformation { get; set; }

    // Movie
    public ICollection<Movie> Movies { get; set; }
}

public record DirectorDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}

public record DirectorCreateDto
{
    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    public ContactInformation ContactInformation { get; set; }
}
