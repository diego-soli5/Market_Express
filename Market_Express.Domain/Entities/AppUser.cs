using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            AppUserRoles = new HashSet<AppUserRole>();
            BinnacleAccesses = new HashSet<BinnacleAccess>();
            BinnacleMovements = new HashSet<BinnacleMovement>();
        }

        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string IdentificationWithoutHypens => Identification;

        public Client Client { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<BinnacleAccess> BinnacleAccesses { get; set; }
        public ICollection<BinnacleMovement> BinnacleMovements { get; set; }
    }
}
