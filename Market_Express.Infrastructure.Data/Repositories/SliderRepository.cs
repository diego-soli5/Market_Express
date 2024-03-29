﻿using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class SliderRepository : GenericRepository<Slider>, ISliderRepository
    {

        public SliderRepository(MARKET_EXPRESSContext context, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(context, configuration,hostingEnvironment)
        { }

        public IQueryable<Slider> GetAllActive()
        {
            return _dbEntity.Where(x => x.Status == EntityStatus.ACTIVADO);
        }
    
    }
}
