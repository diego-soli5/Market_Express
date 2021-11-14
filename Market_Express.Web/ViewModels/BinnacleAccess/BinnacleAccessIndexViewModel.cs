using Market_Express.Application.DTOs.BinnacleAccess;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.BinnacleAccess;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.BinnacleAccess
{
    public class BinnacleAccessIndexViewModel
    {
        public BinnacleAccessQueryFilter Filters { get; set; }
        public Metadata Metadata { get; set; }
        public List<BinnacleAccessDTO> BinnacleAccesses { get; set; }
    }
}
