using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data
{
    public abstract class ConnectionSql
    {
        private readonly IConfiguration _configuration;
        private readonly string _cnn;

        public ConnectionSql(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _cnn = hostingEnvironment.IsDevelopment() ? "Development_SQL" : "Production_SQL";
            _configuration = configuration;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString(_cnn));
        }
    }
}
