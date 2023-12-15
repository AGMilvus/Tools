namespace EmailValidation.Models;

public class EmailEntity
{
    public string Email { get; set; }
    public bool Checked { get; set; }
    public bool Valid { get; set; }
    public bool DoNotUseEmail { get; set; }
    public string? Reason { get; set; } = null!;
}