using Market_Express.Application.DTOs.AppUser;
using System;

namespace Market_Express.Application.DTOs.BinnacleAccess
{
    public class BinnacleAccessDTO
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public AppUserDTO AppUser { get; set; }
    }
}
