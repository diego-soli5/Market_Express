using Market_Express.Domain.Entities;
using System.Linq;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ISliderRepository : IGenericRepository<Slider>
    {
        IQueryable<Slider> GetAllActive();
    }
}
