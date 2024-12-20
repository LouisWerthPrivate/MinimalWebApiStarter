using APITemplate.Services;

namespace APITemplate.ServiceExtentions
{
    /// <summary>
    /// Provides extension methods for adding application services to the IServiceCollection.
    /// </summary>
    public static class ApplicationServicesServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicationService, ApplicationService>();
            return services;
        }
    }
}
