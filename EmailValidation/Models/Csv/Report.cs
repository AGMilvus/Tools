namespace EmailValidation.Models.Csv;

public class Report
{
    public string Email { get; set; }
    public State State { get; set; }
    public string? Reason { get; set; }
}

public enum State
{
    active,
    inactive,
    not_email
}