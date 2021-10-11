using System;

namespace Market_Express.CrossCutting.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(Guid resourceId, string resourceType)
        {
            ResourceId = resourceId;
            ResourceType = resourceType;
        }

        public Guid ResourceId { get; set; }
        public string ResourceType { get; set; }
    }
}
