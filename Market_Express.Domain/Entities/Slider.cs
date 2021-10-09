using System;

namespace Market_Express.Domain.Entities
{
    public class Slider : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
