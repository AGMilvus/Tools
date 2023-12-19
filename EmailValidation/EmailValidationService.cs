using System.Diagnostics;
using EmailValidation.Models;
using EmailValidation.Models.Csv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EmailValidation;

public class EmailValidationService : BackgroundService
{
    private readonly CsvFileService _csvFileService;
    
    private readonly string[] _requestFiles;
    private readonly string[] _reportFiles;
    private readonly string _OverallFile;
    
    private List<EmailEntity> _notInOriginalEmails = [];
    private List<Entity> _notInReportEmails = [];
    private List<Entity> _badEmails = [];
    
    public EmailValidationService(CsvFileService csvFileService, IConfiguration configuration)
    {
        _requestFiles = configuration.GetSection("Files").GetSection("Requests").Get<string[]>()!;
        _reportFiles = configuration.GetSection("Files").GetSection("Reports").Get<string[]>()!;
        _OverallFile = configuration.GetSection("Files")["Overall"]!;
        _csvFileService = csvFileService;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        var requests = new List<Request>();
        foreach (var requestFile in _requestFiles)
        {
            requests.Add(new Request(requestFile).Load());
        }
        Console.WriteLine("Requests loaded. {0}", sw.ElapsedMilliseconds);
        
        var reports = new List<Report>();
        foreach (var reportFile in _reportFiles)
        {
            reports.Add(new Report(reportFile).Load());
        }
        Console.WriteLine("Reports loaded. {0}", sw.ElapsedMilliseconds);
        
        var emails = LoadEmails();
        Console.WriteLine("Emails loaded. {0}", sw.ElapsedMilliseconds);
        
        MergeReport(emails, reports);
        
        Console.WriteLine("Reports merged. {0}", sw.ElapsedMilliseconds);
        
        foreach (var email in emails)
        {
            if(!email.Value.Checked) _notInReportEmails.Add(new Entity(){Email = email.Value.Email});
        }
        // _csvFileService.WriteAll(_badEmails);
        
        Console.WriteLine("End. {0}", sw.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    private Dictionary<string, EmailEntity> LoadEmails()
    {
        var original = CsvFileService.Load<Original>(_OverallFile);
        var emails = new Dictionary<string, EmailEntity>();
        foreach (var o in original)
        {
            if (emails.ContainsKey(o.Email)) continue; 
            emails.Add(o.Email, new EmailEntity { Email = o.Email, DoNotUseEmail = o.DoNotUseEmail });
        }
        return emails;
    }
    
    private void MergeReport(Dictionary<string, EmailEntity> emails, IEnumerable<Report> report)
    {        
        EmailEntity tmp;
        foreach (var r in report)
        {
            if (r.Alls == null) continue;
            foreach (var a in r.Alls)
            {
                if (!emails.TryGetValue(a.Email, out tmp))
                {
                    tmp = new EmailEntity
                    {
                        Email = a.Email,
                        Checked = true,
                        Reason = a.Reason,
                        Valid = a.State == State.active,
                        Comment = "Original not found."
                    };
                    emails.Add(a.Email, tmp);
                    _notInOriginalEmails.Add(tmp);
                }
                else
                {
                    tmp.Checked = true;
                    tmp.Reason = a.Reason;
                    tmp.Valid = a.State == State.active;
                    if (!tmp.Valid) _badEmails.Add(new Entity() { Email = tmp.Email });
                }
            }
        }
    }
}