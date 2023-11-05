using CsvHelper.TypeConversion;
using CsvHelper;
using System.Text.RegularExpressions;

namespace ParseELKData.Converters;

public class ElkRecordConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        string pattern = @"([A-Z][a-z]{2}) (\d{1,2}), (\d{4}) @ ((\d{2}:){2}\d{2}.\d{3})";
        MatchCollection matches = Regex.Matches(text, pattern);
        if (matches.Count == 0)
            throw new ArgumentException("Incorrect ELK Date Format");
        int month = GetMonthIndex(matches[0].Groups[1].Value);
        int day = int.Parse(matches[0].Groups[2].Value);
        int year = int.Parse(matches[0].Groups[3].Value);
        TimeOnly time = TimeOnly.Parse(matches[0].Groups[4].Value);
        return new DateTime(year, month, day, time.Hour, 0, 0); //time.Minute, time.Second); 
    }

    private int GetMonthIndex(string month)
    {
        return month switch
        {
            "Jan" => 1,
            "Feb" => 2,
            "Mar" => 3,
            "Apr" => 4,
            "May" => 5,
            "Jun" => 6,
            "Jul" => 7,
            "Aug" => 8,
            "Sep" => 9,
            "Oct" => 10,
            "Nov" => 11,
            "Dec" => 12,
            _ => throw new ArgumentException("Incorrect month")
        };
    }
}