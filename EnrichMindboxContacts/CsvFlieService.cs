using System.Collections;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EnrichMindboxContacts.Models;

namespace EnrichMindboxContacts;

public class CsvFileService
{
    public static List<T> Load<T>(string path, string delimiter = ",") where T : class
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter
        });
        return csv.GetRecords<T>().ToList();
    }

    public static void Write(string path, IEnumerable data)
    {
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.TypeConverterCache.AddConverter<bool>(new CustomBoolConverter());
        csv.WriteRecords(data);
    }

    private class CustomBoolConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if( value == null )
            {
                return string.Empty;
            }
            return ((bool)(value) ? 1 : 0).ToString();
        }
    }
}