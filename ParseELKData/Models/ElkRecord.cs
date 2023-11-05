using CsvHelper.Configuration.Attributes;
using ParseELKData.Converters;

namespace ParseELKData.Models;
     
[Delimiter(";")]
public class ElkRecord
{
    [TypeConverter(typeof(ElkRecordConverter))]
    [Name("timestamp")]
    public DateTime Timestamp { get; set; }
    [Name("duration")]
    public int Duration { get; set; }
}