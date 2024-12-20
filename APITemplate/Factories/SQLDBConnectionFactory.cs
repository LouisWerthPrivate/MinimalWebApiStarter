using Microsoft.Data.SqlClient;
using System.Data;

namespace APITemplate.Factories
{
    /// <summary>
    /// Factory class for creating SQL database connections.
    /// </summary>
    /// <param name="connectionString">The connection string template with placeholders for username and password.</param>
    /// <param name="userName">The SQL username to replace in the connection string.</param>
    /// <param name="password">The SQL password to replace in the connection string.</param>
    public class SQLDBConnectionFactory(string connectionString, string userName, string password) : IDBConnectionFactory
    {
        /// <summary>
        /// Creates and opens a new SQL database connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the open database connection.</returns>
        public async Task<IDbConnection> CreateConnection(CancellationToken cancellationToken = default)
        {
            var connection = new SqlConnection(connectionString.Replace("**SqlUsername**", userName).Replace("**SqlPassword***", password));
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
