using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace APITemplate.ServiceExtentions
{
    /// <summary>
    /// Extension methods for adding logging services to the IServiceCollection.
    /// </summary>
    public static class LoggingServiceCollectionExtentions
    {
        /// <summary>
        /// Adds application logging services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="config">The configuration to use for setting up the logging services.</param>
        /// <returns>The IServiceCollection with the logging services added.</returns>
        public static IServiceCollection AddApplicationLogging(this IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("AdvancedLogging:HttpLogging", defaultValue: false) == true)
            {
                services.AddHttpLogging(logging =>
                {
                    logging.LoggingFields = HttpLoggingFields.All;
                    logging.RequestBodyLogLimit = 4096;
                    logging.ResponseBodyLogLimit = 4096;
                    logging.CombineLogs = true;
                });
            }
            if (config.GetValue<bool>("Otlp:Active", defaultValue: false) == true)
            {
                services.AddOpenTelemetry()
                   .ConfigureResource(r => r
                       .AddService(
                           serviceName: config.GetValue("Otlp:ServiceName", defaultValue: "http://localhost:4317")!,
                           serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
                           serviceInstanceId: Environment.MachineName))
                   .WithTracing(builder =>
                   {
                       builder
                           .AddHttpClientInstrumentation()
                           .AddAspNetCoreInstrumentation();
                       builder.AddOtlpExporter(otlpOptions =>
                       {
                           otlpOptions.Endpoint = new Uri(config.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                       });
                   })
                   .WithMetrics(builder =>
                   {
                       builder
                           .AddRuntimeInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddAspNetCoreInstrumentation();

                       builder.AddOtlpExporter(otlpOptions =>
                       {
                           otlpOptions.Endpoint = new Uri(config.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                       });
                   })
                   .WithLogging(builder =>
                   {
                       builder.AddOtlpExporter(otlpOptions =>
                       {
                           otlpOptions.Endpoint = new Uri(config.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                       });
                   });
            }
            return services;
        }
    }
}
