using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Address(string name, string detail)
        {
            Name = name;
            Detail = detail;
        }

        public Address()
        {

        }

        public Address(Guid id, string name, string detail, bool inUse)
        {
            Id = id;
            Name = name;
            Detail = detail;
            InUse = inUse;
        }

        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public bool InUse { get; set; }

        public Client Client { get; set; }
    }
}
