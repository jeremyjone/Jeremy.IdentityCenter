using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Ensure;
using Jeremy.IdentityCenter.Database.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "Jeremy.IdentityCenter Server";

            var config = GetConfiguration(args);

            const string outputTemplate =
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                .WriteTo.File(
                    @"LogFiles\log.log",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: outputTemplate, theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                var seed = args.Contains("/seed");
                if (seed)
                {
                    args = args.Except(new[] { "/seed" }).ToArray();
                }

                var host = CreateHostBuilder(args).Build();

                if (seed)
                {
                    Log.Information("Seeding database...");
                    //SeedData.EnsureSeedData(host.Services);
                    await DbMigrations
                        .ApplyMigrationsAsync<IdcIdentityDbContext, IdcConfigurationDbContext,
                            IdcPersistedGrantDbContext, IdcDataProtectionDbContext, IdcIdentityUser,
                            IdcIdentityRole, int>(host.Services);
                    Log.Information("Done seeding database.");
                }

                Log.Information("Starting host...");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location));
                    var env = context.HostingEnvironment;

                    //config.AddJsonFile("databases.json", true, true);
                    //config.AddJsonFile($"databases.{env.EnvironmentName}.json", true, true);

                    if (env.IsDevelopment())
                    {
                        config.AddUserSecrets<Startup>();
                    }

                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseSerilog()
                    .UseStartup<Startup>();
                });


        private static IConfiguration GetConfiguration(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            //.AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
            //.AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

            if (isDevelopment)
            {
                configurationBuilder.AddUserSecrets<Startup>();
            }

            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }
}
