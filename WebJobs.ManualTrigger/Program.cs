using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    static async Task Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var builder = new HostBuilder()
            .ConfigureWebJobs(webJobConfiguration =>
            {
                webJobConfiguration.AddAzureStorageCoreServices();
                webJobConfiguration.AddAzureStorage();
            })
            .UseEnvironment(environment)
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            })
            .ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder.AddConsole();
            })
            .ConfigureServices(s =>
            {
                // Dependency resolution
            })
            .UseConsoleLifetime();

        using (var host = builder.Build())
        {
            var jobHost = host.Services.GetService(typeof(IJobHost)) as JobHost;
            var configuration = host.Services.GetService(typeof(IConfiguration));

            await host.StartAsync();
            await jobHost.CallAsync("DoWork", new Dictionary<string, object>
            {
                { "configuration", configuration}
            });
            await host.StopAsync();
        }
    }

    [NoAutomaticTrigger()]
    public static void DoWork(IConfiguration configuration)
    {
        var value = configuration["Value"];

        Console.WriteLine($"Job executed and the value is {value}");
    }
}