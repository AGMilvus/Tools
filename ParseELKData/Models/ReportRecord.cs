using CsvHelper.Configuration.Attributes;
using ParseELKData.Converters;

namespace ParseELKData.Models;
     
[Delimiter(";")]
public class ReportRecord
{
    [TypeConverter(typeof(ElkRecordConverter))]
    [Name("timestamp")]
    public DateTime Timestamp { get; set; }
    [Name("count")]
    public int Count { get; set; }
    [Name("average(ms)")]
    public int Average { get; set; }
    
}