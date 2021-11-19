using Market_Express.Application.DTOs.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Report
{
    public class ReportArticleViewModel
    {
        public List<ArticleForReportDTO> Articles { get; set; }
        public ReportArticleQueryFilter Filters { get; set; }
        public Metadata Metadata { get; set; }
    }
}
