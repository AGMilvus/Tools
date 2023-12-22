namespace EnrichMindboxContacts.Models;

public class FullNameDiff
{
    public Guid Id { get; set; }
    public string BpmFirstName { get; set; }
    public string BpmMiddleName { get; set; }
    public string BpmLastName { get; set; }
    public string MBFirstName { get; set; }
    public string MBMiddleName { get; set; }
    public string MBLastName { get; set; }
}