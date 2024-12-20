using APITemplate.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APITemplate.ServiceExtentions
{
    /// <summary>
    /// Provides extension methods for adding authentication services to the application.
    /// </summary>
    public static class AuthenticationServiceExtentions
    {
        /// <summary>
        /// Adds authentication services to the specified IServiceCollection.
        /// Configures JWT Bearer and API Key authentication schemes.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="config">The IConfiguration containing the authentication settings.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration config)
        {
            // Configure ApiSecurityOptions from the configuration
            services.Configure<ApiSecurityOptions>(config.GetSection("ApiSecurityOptions"));

            // Add authentication services and configure the default schemes
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Authentication";
                options.DefaultChallengeScheme = "Authentication";
            })
            // Add JWT Bearer authentication scheme
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetValue<string>("ApiSecurityOptions:JwtBeareer:Authority","default"),
                    ValidAudience = config.GetValue<string>("ApiSecurityOptions:JwtBeareer:Audience","default"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("ApiSecurityOptions:JwtBeareer:SymmetricSecurityKey", "bXlzdXBlcnNlY3JldGtleQ==bXlzdXBlcnNlY3JldGtleQ==")))
                };
            })
            // Add API Key authentication scheme
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
            {
                options.AllowedAPIKeys = config.GetValue<string>("ApiSecurityOptions:ApiKey:AllowedApiKeys", "");
            })
            // Add policy scheme to select between JWT Bearer and API Key based on the presence of the X-Api-Key header
            .AddPolicyScheme("Authentication", "Bearer or ApiKey", options =>
            {
                options.ForwardDefaultSelector = context => context.Request.Headers.ContainsKey(config.GetValue<string>("ApiSecurityOptions:ApiKey:Header", "X-Api-Key")) ? "ApiKey" : "Bearer";
            });

            // Add authorization policies for Bearer and API Key schemes
            services.AddAuthorizationBuilder()
                // Add authorization policies for Bearer and API Key schemes
                .AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                })
                // Add authorization policies for Bearer and API Key schemes
                .AddPolicy("ApiKey", policy =>
                {
                    policy.AddAuthenticationSchemes("ApiKey");
                    policy.RequireAuthenticatedUser();
                });

            return services;
        }
    }
}
