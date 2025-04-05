using Microsoft.Data.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class DatabaseContext
    {

        private readonly IConfiguration _configuration;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_configuration.GetConnectionString("ConnectionString"));
        }
    }

}

