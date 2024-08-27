using System.ComponentModel.DataAnnotations;

namespace Api.Models;

#nullable disable

public class Genre {

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; }

    // Movie
    public ICollection<Movie> Movies { get; set; }
}

public class GenreResponseDto {
    
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 3)]
    public string Name { get; set; }
}
