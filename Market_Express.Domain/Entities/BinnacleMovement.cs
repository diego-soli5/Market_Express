using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BinnacleMovement : BaseEntity
    {
        public Guid AppUserId { get; set; }
        public DateTime MovementDate { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }

        public AppUser AppUser { get; set; }
    }
}
