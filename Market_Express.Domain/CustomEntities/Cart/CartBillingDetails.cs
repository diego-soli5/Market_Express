namespace Market_Express.Domain.CustomEntities.Cart
{
    public class CartBillingDetails : Entities.Cart
    {
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }

        public decimal Total => SubTotal - Discount;
    }
}
