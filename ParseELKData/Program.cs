using CsvHelper.Configuration.Attributes;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ParseELKData;

internal class Program
{
    static void Main(string[] args)
    {
        var data = ReadData();
        
         var h = data.GroupBy(p => p.Timestamp)
             .Select(g => new {Timstamp = g.Key, Count = g.Count(), Avg = g.Average(p => p.Duration)});
        
         using var writer = new StreamWriter("output.csv");
         using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
         csv.WriteRecords(h);
        
        //SaveData(data);

    }
    
     private static List<ElkRecord> ReadData()
     {
         using var reader = new StreamReader(@"BeesMsgDuration.csv");
         List<ElkRecord> result = new List<ElkRecord>();
         reader.ReadLine();
         while (!reader.EndOfStream)
         {
             var values = reader.ReadLine().Split(';');
             result.Add(new ElkRecord
             {
                 Timestamp = ParseElkDateTime(values[0]),
                 Duration = int.Parse(values[1])
             });
         }
         return result;
     }
    
     private static void SaveData(IEnumerable<ElkRecord> data)
     {
         using var writer = new StreamWriter("output.csv");
         using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
         csv.WriteRecords(data);
     }

     private static DateTime ParseElkDateTime(string text)
     {
         string pattern = @"([A-Z][a-z]{2}) (\d{1,2}), (\d{4}) @ ((\d{2}:){2}\d{2}.\d{3})";
         MatchCollection matches = Regex.Matches(text, pattern);
         if (matches.Count == 0)
             throw new ArgumentException("Incorrect ELK Date Format");
         int month = GetMonthIndex(matches[0].Groups[1].Value);
         int day = int.Parse(matches[0].Groups[2].Value);
         int year = int.Parse(matches[0].Groups[3].Value);
         TimeOnly time = TimeOnly.Parse(matches[0].Groups[4].Value);
         return new DateTime(year, month, day, time.Hour, 0, 0); 
     }

     private static int GetMonthIndex(string month)
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
     
     [Delimiter(";")]
     private class ElkRecord
    {
        public DateTime Timestamp { get; set; }
        public int Duration { get; set; }
    }
}
