using System.Diagnostics;
using System.Text;
using EmailValidation;
using EmailValidation.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SplitCsv;

public class CsvSplittingService : BackgroundService
{
    private readonly string _OverallFileName;
    private readonly string _OutputFileName;
    
    
    public CsvSplittingService(CsvFileService csvFileService, IConfiguration configuration)
    {
        var filesSection = configuration.GetSection("Files");
        _OverallFileName = filesSection["Overall"]!;
        _OutputFileName = filesSection["Output"]!;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        var emails = CsvFileService.Load<Entity>(GetFullPath(_OverallFileName));
        Console.WriteLine("Emails loaded. {0}", sw.ElapsedMilliseconds);

        var tmp = ChunkBy(emails, 800000);

        int i = 0;
        foreach (var t in tmp)
        {
            CsvFileService.Write(GetFullPath(_OutputFileName, i++.ToString()), t);
        }
        
        Console.WriteLine("End. {0}", sw.ElapsedMilliseconds);
        return Task.CompletedTask;
    }
    
    public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize) 
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
    
    private string GetFullPath(string file, string suffix = "")
    {
        var sb = new StringBuilder("csv/");
        sb.Append(file);
        sb.Append(suffix);
        sb.Append(".csv");
        return sb.ToString();
    }
}