using System.ComponentModel.DataAnnotations;

namespace EmailValidation.Models;

public class EmailEntity : Entity
{
    [Key]
    public Guid Id;
    public bool Checked { get; set; }
    public bool Valid { get; set; }
    public bool DoNotUseEmail { get; set; }
    public string? Reason { get; set; } = null!;
    public string? Comment { get; set; } = null!;
}