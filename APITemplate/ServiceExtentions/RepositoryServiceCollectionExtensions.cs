using APITemplate.Repositories;

namespace APITemplate.ServiceExtentions
{
    /// <summary>
    /// Provides extension methods for adding resource services to the IServiceCollection.
    /// </summary>
    public static class RepositoryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds repository services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The IServiceCollection with the repository services added.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository, Repository>();
            return services;
        }
    }
}
