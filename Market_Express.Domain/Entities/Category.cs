#nullable disable

using System;

namespace Market_Express.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate{ get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
