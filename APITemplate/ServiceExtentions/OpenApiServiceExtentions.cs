using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace APITemplate.ServiceExtentions
{
    /// <summary>
    /// Provides extension methods for configuring OpenAPI documentation services.
    /// </summary>
    public static class OpenApiServiceExtentions
    {
        /// <summary>
        /// Adds OpenAPI documentation services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddApplicationOpenApiDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<SecuritySchemeTransformer>();
            });
            return services;
        }
    }

    /// <summary>
    /// A transformer that adds security schemes to the OpenAPI document.
    /// </summary>
    internal sealed class SecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        /// <summary>
        /// Transforms the OpenAPI document by adding security schemes.
        /// </summary>
        /// <param name="document">The OpenAPI document to transform.</param>
        /// <param name="context">The context for the transformation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                },
                ["ApiKey"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "X-Api-Key",
                    In = ParameterLocation.Header
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
            return Task.CompletedTask;
        }
    }
}

