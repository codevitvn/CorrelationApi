// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Reflection.Metadata;
using Dashboard;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = CreateHostBuilder(args);

var host = builder.Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Host created.");

await host.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(x =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                x.AddConsumers(entryAssembly);
                x.UsingRabbitMq((context, configurator) =>
                {
                    //need forward port 5672
                    configurator.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    configurator.ConfigureEndpoints(context);
                });
                x.AddConsumer<GettingStartedConsumer>();
            });
        })
        .ConfigureLogging((context, builder) =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serilog.json", false, true)
                .Build();

            // Add services to the container.
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .CreateLogger();
            
            builder.ClearProviders();
            builder.AddSerilog(logger);
        });