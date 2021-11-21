using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public abstract class ADORepository : ConnectionSql
    {
        public ADORepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(configuration, hostingEnvironment)
        { }

        protected async Task<DataTable> ExecuteQuery(string sSpName, SqlParameter[] arrParameters = null)
        {
            using (var oConnection = GetConnection())
            {
                using (var oCommand = new SqlCommand(sSpName, oConnection))
                {
                    oCommand.CommandType = CommandType.StoredProcedure;

                    if (arrParameters != null)
                        oCommand.Parameters.AddRange(arrParameters);

                    await oConnection.OpenAsync();

                    using (var oTable = new DataTable())
                    {
                        using (var oReader = await oCommand.ExecuteReaderAsync())
                        {
                            oTable.Load(oReader);

                            return oTable;
                        }
                    }
                }
            }
        }

        protected int ExecuteNonQuery(string sSpName, SqlParameter[] arrParameters)
        {
            using (var oConnection = GetConnection())
            {
                oConnection.Open();

                using (var oCommand = new SqlCommand(sSpName, oConnection))
                {
                    oCommand.Parameters.AddRange(arrParameters);

                    int iResult = oCommand.ExecuteNonQuery();

                    return iResult;
                }
            }
        }
    }
}
