using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class CartDetail : BaseEntity
    {
        public CartDetail(Guid id,Guid cartId, Guid articleId, decimal quantity)
        {
            Id = id;
            CartId = cartId;
            ArticleId = articleId;
            Quantity = quantity;
        }

        public CartDetail()
        {

        }

        public Guid CartId { get; set; }
        public Guid ArticleId { get; set; }
        public decimal Quantity { get; set; }

        public Article Article { get; set; }
        public Cart Cart { get; set; }
    }
}
