using System.Data;

namespace APITemplate.Factories
{
    /// <summary>
    /// Interface for creating database connections.
    /// </summary>
    public interface IDBConnectionFactory
    {
        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the database connection.</returns>
        Task<IDbConnection> CreateConnection(CancellationToken cancellationToken = default);
    }
}
