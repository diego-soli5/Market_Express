using Market_Express.Application.DTOs.BinnacleMovement;
using Market_Express.Domain.QueryFilter.BinnacleMovement;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.BinnacleMovement
{
    public class MovementReportViewModel
    {
        public List<BinnacleMovementDTO> BinnacleMovements { get; set; }
        public BinnacleMovementQueryFilter Filters { get; set; }
    }
}
