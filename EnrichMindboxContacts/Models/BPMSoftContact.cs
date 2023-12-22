using CsvHelper.Configuration.Attributes;

namespace EnrichMindboxContacts.Models;

public class BpmSoftContact
{
    public Guid Id { get; set; }
    [Optional]
    public string Surname { get; set; }
    [Optional]
    public string GivenName { get; set; }
    [Optional]
    public string MiddleName { get; set; }
    [Optional]
    public string Email { get; set; }
    [Optional]
    public string MobilePhone { get; set; }
}