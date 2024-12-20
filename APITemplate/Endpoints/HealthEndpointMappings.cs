using HealthChecks.UI.Client;

namespace APITemplate.Endpoints
{
    public static class HealthEndpointMappings
    {
        public static WebApplication MapApplicationHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions 
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
    }
}
