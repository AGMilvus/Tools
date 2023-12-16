using System.Diagnostics;
using EmailValidation.EntityFrameworkCore;
using EmailValidation.EntityFrameworkCore.Repositories;
using EmailValidation.Models;
using EmailValidation.Models.Csv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EmailValidation;

public class EmailValidationService : BackgroundService
{
    private readonly CsvFileService _csvFileService;
    private IEmailRepository _repo;

    private List<EmailEntity> _notInOriginalEmails = [];
    private List<Entity> _notInReportEmails = [];
    private List<EmailEntity> _badEmails = [];
    
    public EmailValidationService(CsvFileService csvFileService, IEmailRepository repo)
    {
        _csvFileService = csvFileService;
        _repo = repo;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        var emails = LoadEmails();

        Console.WriteLine("Emails loaded. {0}", sw.ElapsedMilliseconds);
        
        var report = _csvFileService.LoadReportAll();
        
        Console.WriteLine("Reports loaded. {0}", sw.ElapsedMilliseconds);

        MergeReport(emails, report);
        
        Console.WriteLine("Reports merged. {0}", sw.ElapsedMilliseconds);
        
        foreach (var email in emails)
        {
            if(!email.Value.Checked) _notInReportEmails.Add(new Entity(){Email = email.Value.Email});
        }
        _csvFileService.WriteAll(_notInReportEmails);
        
        Console.WriteLine("End. {0}", sw.ElapsedMilliseconds);
        return Task.CompletedTask;
    }

    private Dictionary<string, EmailEntity> LoadEmails()
    {
        var original = _csvFileService.LoadOriginalAll();
        var emails = new Dictionary<string, EmailEntity>();
        foreach (var o in original)
        {
            emails.Add(o.Email, new EmailEntity { Email = o.Email, DoNotUseEmail = o.DoNotUseEmail });
        }
        return emails;
    }
    
    private void MergeReport(Dictionary<string, EmailEntity> emails, IEnumerable<Report> report)
    {        
        EmailEntity tmp;
        foreach (var r in report)
        {
            if (!emails.TryGetValue(r.Email, out tmp))
            {
                tmp = new EmailEntity
                {
                    Email = r.Email,
                    Checked = true,
                    Reason = r.Reason,
                    Valid = r.State == State.active,
                    Comment = "Original not found."
                };
                emails.Add(r.Email, tmp);
                _notInOriginalEmails.Add(tmp);
            }
            else
            {
                tmp.Checked = true;
                tmp.Reason = r.Reason;
                tmp.Valid = r.State == State.active;
                if(!tmp.Valid) _badEmails.Add(tmp);
            }
        }
    }
}