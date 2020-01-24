using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CartService.Application.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public ConnectionFactory(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection GetConnection()
        {
            var result = new SqlConnection(connectionString);
            result.Open();

            return result;
        }
    }
}
