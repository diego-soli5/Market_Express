using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public string Image { get; set; }
    }
}
