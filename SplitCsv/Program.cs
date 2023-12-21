using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SplitCsv;

namespace EmailValidation;

internal class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<CsvFileService>();
                services.AddHostedService<CsvSplittingService>();
            })
            .Build();
 
        host.Run();
    }
}