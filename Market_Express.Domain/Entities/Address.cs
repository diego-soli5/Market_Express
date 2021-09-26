using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }

        public Client Client { get; set; }
    }
}
