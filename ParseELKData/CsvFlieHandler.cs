using System.Collections;
using System.Globalization;
using CsvHelper;
using ParseELKData.Models;

namespace ParseELKData;

public interface ICsvFileHandler
{
    public List<ElkRecord> ReadAll();
    public void WriteAll(IEnumerable data);
    
}

public class CsvFileHandler : ICsvFileHandler
{
    private readonly string _inputFilePath;
    private readonly string _outputFilePath;

    public CsvFileHandler(string inputFilePath, string outputFilePath)
    {
        _inputFilePath = inputFilePath;
        _outputFilePath = outputFilePath;
    }

    public List<ElkRecord> ReadAll()
    {
        using var reader = new StreamReader(_inputFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<ElkRecord>().ToList();
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