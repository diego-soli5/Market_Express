using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.QueryFilter.Home
{
    public class HomeSearchQueryFilter
    {
        public string Q { get; set; }
        [BindProperty(Name = "Category")]
        public List<Guid> Category { get; set; }
        public int? PriceMax { get; set; }
        public int? PriceMin { get; set; }
    }
}
