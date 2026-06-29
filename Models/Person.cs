using System.ComponentModel.DataAnnotations;

namespace PersonApp.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Jméno")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Příjmení")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Telefon")]
    [Phone]
    [StringLength(30)]
    public string? Phone { get; set; }
}
