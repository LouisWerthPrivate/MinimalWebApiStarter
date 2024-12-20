using APITemplate.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APITemplate.ServiceExtentions
{
    public static class HealthCheckBuilderExtentions
    {
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("HealthChecks:Enabled"))
            {
                services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddCheck<ApplicationConfigurationHealthCheck>("Configuration");
            }
            return services;
        }   
    }
}
