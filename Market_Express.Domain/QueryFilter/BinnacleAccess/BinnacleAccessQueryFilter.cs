using Microsoft.AspNetCore.Mvc;
using System;

namespace Market_Express.Domain.QueryFilter.BinnacleAccess
{
    public class BinnacleAccessQueryFilter : PaginationQueryFilter
    {
        [BindProperty(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [BindProperty(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [BindProperty(Name = "User")]
        public string User { get; set; }
    }
}
