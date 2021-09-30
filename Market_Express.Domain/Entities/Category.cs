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

        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
