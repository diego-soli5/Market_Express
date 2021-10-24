using System;

namespace Market_Express.Application.DTOs.Permission
{
    class PermissionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PermissionCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
