using Market_Express.Application.DTOs.AppUser;
using System;

namespace Market_Express.Application.DTOs.BinnacleMovement
{
    public class BinnacleMovementDTO
    {
        public Guid Id { get; set; }
        public string PerformedBy { get; set; }
        public DateTime MovementDate { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }

        public AppUserDTO AppUser { get; set; }
    }
}
