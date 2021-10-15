using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Articles = new HashSet<Article>();
        }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Category(Guid id,string name, string description,EntityStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
