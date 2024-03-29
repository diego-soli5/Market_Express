﻿using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration, hostingEnvironment)
        { }

    }
}
