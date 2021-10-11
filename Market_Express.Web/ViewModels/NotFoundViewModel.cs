using System;

namespace Market_Express.Web.ViewModels
{
    public class NotFoundViewModel
    {
        public NotFoundViewModel()
        {

        }

        public NotFoundViewModel(Guid resourceId, string resourceType)
        {
            ResourceId = resourceId;
            ResourceType = resourceType;
        }

        public Guid ResourceId { get; set; }
        public string ResourceType { get; set; }
    }
}
