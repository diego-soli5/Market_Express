using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public AppUser(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public AppUser(string name, string identification, string email, string phone, AppUserType type)
        {
            Name = name;
            Identification = identification;
            Email = email;
            Phone = phone;
            Type = type;
        }

        public AppUser(Guid id, string identification, string email, string phone, EntityStatus status, AppUserType type)
        {
            Id = id;
            Identification = identification;
            Email = email;
            Phone = phone;
            Status = status;
            Type = type;
        }

        public string Name { get; set; }
        public string Alias { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public AppUserType Type { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string IdentificationWithoutHypens => Regex.Replace(Identification, @"[^\w\.@]", "",
                                RegexOptions.None, TimeSpan.FromSeconds(1.5));

        public Client Client { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<BinnacleAccess> BinnacleAccesses { get; set; }
        public ICollection<BinnacleMovement> BinnacleMovements { get; set; }
    }
}
