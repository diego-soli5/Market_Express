using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.BinnacleMovement;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class BinnacleMovementRepository : GenericRepository<BinnacleAccess>, IBinnacleMovementRepository
    {
        private const string _Sp_BinnacleMovement_GetAllPaginated = "Sp_BinnacleMovement_GetAllPaginated";
        private const string _Sp_BinnacleMovement_GetAllForReport = "Sp_BinnacleMovement_GetAllForReport";

        public BinnacleMovementRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<SQLServerPagedList<BinnacleMovement>> GetAllPaginated(BinnacleMovementQueryFilter filters)
        {
            List<BinnacleMovement> lstBinnacleMovement = new();

            SqlParameter pTotalPages = new("@totalPages", 0)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter pTotalCount = new("@totalCount", 0)
            {
                Direction = ParameterDirection.Output
            };

            var arrParams = new[]
            {
                new SqlParameter("@type",filters.Type),
                new SqlParameter("@startdate",filters.StartDate),
                new SqlParameter("@enddate",filters.EndDate),
                new SqlParameter("@name",filters.User),
                new SqlParameter("@ignoreSystem",filters.IgnoreSystem),
                new SqlParameter("@pageNumber",filters.PageNumber-1),
                new SqlParameter("@pageSize",filters.PageSize),
                pTotalPages,
                pTotalCount
            };

            var dtResult = await ExecuteQuery(_Sp_BinnacleMovement_GetAllPaginated, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstBinnacleMovement.Add(new BinnacleMovement
                {
                    Id = (Guid)oRow["Id"],
                    Type = oRow["Type"].ToString(),
                    Detail = oRow["Detail"].ToString(),
                    MovementDate = (DateTime)oRow["MovementDate"],
                    PerformedBy = oRow["PerformedBy"].ToString(),
                    AppUser = new AppUser { Identification = oRow["Identification"].ToString() }
                });
            }

            return new SQLServerPagedList<BinnacleMovement>(lstBinnacleMovement, filters.PageNumber.Value, filters.PageSize.Value, Convert.ToInt32(pTotalPages.Value), Convert.ToInt32(pTotalCount.Value));
        }

        public async Task<List<BinnacleMovement>> GetAllForReport(BinnacleMovementQueryFilter filters)
        {
            List<BinnacleMovement> lstBinnacleMovement = new();

            var arrParams = new[]
            {
                new SqlParameter("@type",filters.Type),
                new SqlParameter("@startdate",filters.StartDate),
                new SqlParameter("@enddate",filters.EndDate),
                new SqlParameter("@name",filters.User),
                new SqlParameter("@ignoreSystem",filters.IgnoreSystem)
            };

            var dtResult = await ExecuteQuery(_Sp_BinnacleMovement_GetAllForReport, arrParams);

            foreach (DataRow oRow in dtResult.Rows)
            {
                lstBinnacleMovement.Add(new BinnacleMovement
                {
                    Id = (Guid)oRow["Id"],
                    Type = oRow["Type"].ToString(),
                    Detail = oRow["Detail"].ToString(),
                    MovementDate = (DateTime)oRow["MovementDate"],
                    PerformedBy = oRow["PerformedBy"].ToString(),
                    AppUser = new AppUser { Identification = oRow["Identification"].ToString() }
                });
            }

            return lstBinnacleMovement;
        }
    }
}
