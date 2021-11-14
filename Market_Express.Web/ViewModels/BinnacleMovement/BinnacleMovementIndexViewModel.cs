using Market_Express.Application.DTOs.BinnacleMovement;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.BinnacleMovement;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.BinnacleMovement
{
    public class BinnacleMovementIndexViewModel
    {
        public BinnacleMovementQueryFilter Filters { get; set; }
        public Metadata Metadata { get; set; }
        public List<BinnacleMovementDTO> BinnacleMovements { get; set; }
    }
}
