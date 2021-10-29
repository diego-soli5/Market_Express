using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private const string _Sp_Role_GetAllByUserId = "Sp_Role_GetAllByUserId";

        public RoleRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<List<Role>> GetAllByUserId(Guid id)
        {
            List<Role> lstRoles = new();

            var arrParams = new[]
            {
                new SqlParameter("@userId",id)
            };

            var dtResult = await ExecuteQuery(_Sp_Role_GetAllByUserId,arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstRoles.Add(new Role
                {
                    Id = (Guid)oRow["Id"],
                    Name = oRow["Name"].ToString(),
                    Description = oRow["Description"].ToString(),
                    CreationDate = (DateTime)oRow["CreationDate"],
                    ModificationDate = oRow["ModificationDate"] is not DBNull ? (DateTime)oRow["ModificationDate"] : null,
                    AddedBy = oRow["AddedBy"].ToString(),
                    ModifiedBy = oRow["ModifiedBy"].ToString()
                });
            }

            return lstRoles;
        }
    }
}
