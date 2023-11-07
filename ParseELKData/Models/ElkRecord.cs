using CsvHelper.Configuration.Attributes;
using ParseELKData.Converters;

namespace ParseELKData.Models;

public class ElkRecord
{
    [TypeConverter(typeof(ElkRecordConverter))]
    [Name("@timestamp")]
    public DateTime Timestamp { get; set; }
    [Name("message.duration")]
    public int Duration { get; set; }
}