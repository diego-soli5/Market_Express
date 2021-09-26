using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            BinnacleAccess = new HashSet<BinnacleAccess>();
            BinnacleMovements = new HashSet<BinnacleMovement>();
            AppUserRoles = new HashSet<AppUserRole>();
        }

        public string Name { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        #region HELPER PROP
        public string IdentificationWithoutHyphens
        {
            get
            {
                return Identification.Trim().Replace('-', Convert.ToChar(string.Empty));
            }
        }
        #endregion

        public Client Client { get; set; }
        public IEnumerable<BinnacleAccess> BinnacleAccess { get; set; }
        public IEnumerable<BinnacleMovement> BinnacleMovements { get; set; }
        public IEnumerable<AppUserRole> AppUserRoles { get; set; }
    }
}
