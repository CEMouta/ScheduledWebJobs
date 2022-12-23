using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new HostBuilder()
    .ConfigureWebJobs(webJobConfiguration =>
    {
        webJobConfiguration.AddAzureStorageCoreServices();
        webJobConfiguration.AddAzureStorage();
        webJobConfiguration.AddTimers();
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
    await host.RunAsync();
}