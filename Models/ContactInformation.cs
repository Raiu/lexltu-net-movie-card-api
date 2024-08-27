using System.ComponentModel.DataAnnotations;

namespace Api.Models;

#nullable disable

public class ContactInformation {

    [Key]
    public int Id { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    // Director
    public int DirectorId { get; set; }
}
