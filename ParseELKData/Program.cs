using CsvHelper.Configuration.Attributes;
using CsvHelper;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ParseELKData.Models;

namespace ParseELKData;

internal class Program
{
    private static string _inputFilePath = "input.csv";
    private static string _outputFilePath = "output.csv";

    static void Main(string[] args)
    {
        if (args.Length != 0)
            _inputFilePath = args[0];
        var csvHandler = new CsvFileHandler(_inputFilePath, _outputFilePath);
        var data = csvHandler.ReadAll();
        
        var result = data.GroupBy(p => p.Timestamp)
             .Select(g => new ReportRecord{Timestamp = g.Key, Count = g.Count(), Average = (int)Math.Floor(g.Average(p => p.Duration))});
        
        csvHandler.WriteAll(result);
    }
}
