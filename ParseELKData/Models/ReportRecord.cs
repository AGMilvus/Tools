using System.Globalization;
using CsvHelper.Configuration.Attributes;
using ParseELKData.Converters;

namespace ParseELKData.Models;
     
[Delimiter(";")]
public class ReportRecord
{
    [TypeConverter(typeof(ReportElkRecordConverter))]
    [Name("timestamp")]
    public DateTime Timestamp { get; set; }
    [Name("count")]
    public int Count { get; set; }
    [Name("average(ms)")]
    public int Average { get; set; }
    
}