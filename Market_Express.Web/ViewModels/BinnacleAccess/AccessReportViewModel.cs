using Market_Express.Application.DTOs.BinnacleAccess;
using Market_Express.Domain.QueryFilter.BinnacleAccess;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.BinnacleAccess
{
    public class AccessReportViewModel
    {
        public List<BinnacleAccessDTO> BinnaclesAccesses { get; set; }
        public BinnacleAccessQueryFilter Filters { get; set; }
    }
}
