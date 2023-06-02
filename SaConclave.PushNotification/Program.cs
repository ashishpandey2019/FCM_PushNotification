using Microsoft.AspNetCore.Hosting;
using SaConclave.PushNotification;
using Serilog;
using Serilog.Events;

public class Program
{
    public static async Task Main(string[] args)
    {
        #region "Logger"

        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        //  .WriteTo.Debug()
        //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
        //{
        //    AutoRegisterTemplate = true,
        //    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
        //})
        .WriteTo.Console()
        .WriteTo.File(path: "Logs/log.log", LogEventLevel.Information, shared: true, rollingInterval: RollingInterval.Day)
        .CreateLogger();

        #endregion "Logger"

        try
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }
        catch (Exception ee)
        {
            Log.Fatal(ee, "Application CRASHED");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}