using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        private const string _Sp_AppUser_GetPermissions = "Sp_AppUser_GetPermissions";

        public AppUserRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
        { }

        public async Task<List<Permission>> GetPermissionList(Guid id)
        {
            List<Permission> lstPermissions = new();

            var arrParams = new[]
            {
                new SqlParameter("@Id",id)
            };

            var oDTResult = await ExecuteQuery(_Sp_AppUser_GetPermissions, arrParams);

            foreach (DataRow oRow in oDTResult.Rows)
            {
                lstPermissions.Add(new Permission
                {
                    Id = (Guid)oRow["Id"],
                    PermissionCode = oRow["PermissionCode"].ToString(),
                    Name = oRow["Name"].ToString(),
                    Description = oRow["Description"].ToString(),
                    Type = oRow["Type"].ToString()
                });
            }

            return lstPermissions;
        }
    }
}
