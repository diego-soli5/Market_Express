using Market_Express.Domain.Enumerations;

namespace Market_Express.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public string Image { get; set; }
    }
}
