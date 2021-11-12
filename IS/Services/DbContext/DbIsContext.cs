using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IS.Services.DbContext
{
    public class DbIsContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbIsContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbIsConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
