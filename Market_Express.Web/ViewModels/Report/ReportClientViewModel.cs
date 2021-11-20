using Market_Express.Application.DTOs.Client;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Report
{
    public class ReportClientViewModel
    {
        public List<ClientForReportDTO> Clients { get; set; }
        public Metadata Metadata { get; set; }
        public ReportClientQueryFilter Filters { get; set; }
    }
}
