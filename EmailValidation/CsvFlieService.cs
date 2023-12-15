using System.Collections;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using EmailValidation.Models.Csv;
using Microsoft.Extensions.Configuration;

namespace EmailValidation;

public interface ICsvFileService
{
    public IEnumerable<Original> LoadOriginalAll();
    public IEnumerable<Report> LoadReportAll();
    public void WriteAll(IEnumerable data);
}

public class CsvFileService : ICsvFileService
{
    private readonly string _originalFilePath;
    private readonly string[] _reportFilePath;
    private readonly string _outputFilePath;
    
    public CsvFileService(IConfiguration configuration)
    {
        var config = configuration.GetSection("Files");
        _originalFilePath = config["Original"];
        _reportFilePath = config.GetSection("Report").Get<string[]>();
        _outputFilePath = config["Output"];
    }

    public IEnumerable<Original> LoadOriginalAll()
    {
        using var reader = new StreamReader(_originalFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<Original>().ToList();
    }

    public IEnumerable<Report> LoadReportAll()
    {
        var report = new List<Report>();
        foreach (var file in _reportFilePath)
        {
            using var reader = new StreamReader(file);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            report.AddRange(csv.GetRecords<Report>());
        }
        return report;
    }

    public void WriteAll(IEnumerable data)
    {
        using var writer = new StreamWriter(_outputFilePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });
        csv.WriteRecords(data);
    }
}