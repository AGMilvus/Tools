using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EmailValidation;

public class EmailValidationService : BackgroundService
{
    private readonly CsvFileService _csvFileService;
    
    public EmailValidationService(CsvFileService csvFileService)
    {
        _csvFileService = csvFileService;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var o = _csvFileService.LoadOriginalAll();
        var r = _csvFileService.LoadReportAll();
        throw new NotImplementedException();
    }
}