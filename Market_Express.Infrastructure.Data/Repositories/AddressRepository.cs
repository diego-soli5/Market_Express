using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private const string _Sp_Address_GetAllByClient = "Sp_Address_GetAllByClient";

        public AddressRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
        { }

        public async Task<Address> GetSelectedForUseByUserId(Guid userId)
        {
            return await _dbEntity.FirstOrDefaultAsync(a => a.Client.AppUserId == userId && a.InUse);
        }

        public async Task<IEnumerable<Address>> GetAllByUserId(Guid id)
        {
            List<Address> lstAddress = new();

            var arrParams = new[]
            {
                new SqlParameter("@UserId",id)
            };

            var dtResult = await ExecuteQuery(_Sp_Address_GetAllByClient, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstAddress.Add(new Address
                {
                    Id = (Guid)oRow["Id"],
                    ClientId = (Guid)oRow["ClientId"],
                    Name = oRow["Name"].ToString(),
                    Detail = oRow["Detail"].ToString(),
                    InUse = (bool)oRow["InUse"]
                });
            }

            return lstAddress;
        }
    }
}
