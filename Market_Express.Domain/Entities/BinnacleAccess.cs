using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BinnacleAccess : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public AppUser AppUser { get; set; }
    }
}
