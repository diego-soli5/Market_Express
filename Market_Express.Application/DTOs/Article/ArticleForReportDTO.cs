using Market_Express.Domain.Enumerations;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleForReportDTO
    {
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int SoldUnitsCount { get; set; }
        public EntityStatus Status { get; set; }
    }
}
