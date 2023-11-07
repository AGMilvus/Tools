using CsvHelper;
using CsvHelper.TypeConversion;

namespace ParseELKData.Converters;

public class ReportElkRecordConverter : DefaultTypeConverter
{
    public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return ((DateTime)value).ToString("dd/MM/yyyy HH:mm");
    }
}