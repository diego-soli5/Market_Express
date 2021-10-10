using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Domain.Entities
{
    public class Slider : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
