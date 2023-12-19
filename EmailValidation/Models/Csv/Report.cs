using System.Text;
using CsvHelper.Configuration.Attributes;

namespace EmailValidation.Models.Csv;

public class Report
{
    private readonly string _basePath;

    public List<Active>? Actives { get; set;}
    public List<Inactive>? Inactives { get; set;}
    public List<All>? Alls { get; set;}
    
    
    public int allActive = 0;
    public int allInactive = 0;
    public int allNot_email = 0;

    public Report(string  basePath)
    {
        _basePath = basePath;
    }
    
    public Report Load()
    {
        Actives = CsvFileService.Load<Active>(GetFullPath(Active.FileName));
        Inactives = CsvFileService.Load<Inactive>(GetFullPath(Inactive.FileName));
        Alls = CsvFileService.Load<All>(GetFullPath(All.FileName));
        Count();
        return this;
    }
    
    private string GetFullPath(string file)
    {
        var sb = new StringBuilder(_basePath);
        sb.Append('/');
        sb.Append(file);
        return sb.ToString();
    }
    
    private void Count()
    {
        foreach (var all in Alls!)
        {
            switch (all.State)
            {
                case State.active: allActive++;
                    break;
                case State.inactive: allInactive++;
                    break;
                case State.not_email: allNot_email++;
                    break;
                default:
                    throw new ArgumentException("Unknown state");
            }
        }
    }
}

public class Active : Entity
{
    [Ignore]
    public const string FileName = "active.csv";
}

public class Inactive : Entity
{
    [Ignore]
    public const string FileName = "inactive.csv";

    public string? Reason { get; set; }
}

public class All : Inactive
{
    [Ignore]
    public new const string FileName = "all.csv";

    public State? State { get; set; }
}


public enum State
{
    active,
    inactive,
    not_email
}