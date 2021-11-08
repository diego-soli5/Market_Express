using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Cart;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Cart
{
    public class CartIndexViewModel
    {
        public CartBillingDetailsDTO Cart { get; set; }
        public List<ArticleForCartDetailsDTO> Articles { get; set; }
    }
}
