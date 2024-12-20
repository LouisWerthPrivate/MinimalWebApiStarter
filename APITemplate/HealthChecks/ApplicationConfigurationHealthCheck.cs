using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APITemplate.HealthChecks
{
    /// <summary>
    /// Health check to verify the application configuration was correctly configured an replaced during start-up.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ApplicationConfigurationHealthCheck"/> class.
    /// </remarks>
    /// <param name="configuration">The application configuration.</param>
    public class ApplicationConfigurationHealthCheck(IConfiguration _configuration) : IHealthCheck
    {

        /// <summary>
        /// Performs the health check operation.
        /// </summary>
        /// <param name="context">The context in which the health check is being run.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> representing the result of the health check.</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            string connectionString = _configuration.GetValue<string>("Repositories:AlertSqlRepository:ConnectionString", "notconfigured");
            if (connectionString != "notconfigured" && !connectionString.Contains("localhost"))
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(description: "The SQL Connection string was not properly replaced by secrets implementation"));
            }
        }
    }
}
