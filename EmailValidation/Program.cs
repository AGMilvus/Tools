using EmailValidation.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailValidation;

internal class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<CsvFileService>();
                services.AddHostedService<EmailValidationService>();
                services.AddSqlite<AppDbContext>(hostContext.Configuration.GetConnectionString("DefaultConnection"),
                    null, builder => builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            })
            .Build();
 
        host.Run();
    }
}