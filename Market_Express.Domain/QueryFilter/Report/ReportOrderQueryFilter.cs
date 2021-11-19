using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Market_Express.Domain.QueryFilter.Report
{
    public class ReportOrderQueryFilter : PaginationQueryFilter
    {
        [BindProperty(Name = "ClientName")]
        public string ClientName { get; set; }

        [BindProperty(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [BindProperty(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [BindProperty(Name = "Status")]
        public OrderStatus? Status { get; set; }
    }
}
