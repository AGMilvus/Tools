namespace EmailValidation.Models.Csv;

public class Report : Entity
{
    public State State { get; set; }
    public string? Reason { get; set; }
}

public enum State
{
    active,
    inactive,
    not_email
}