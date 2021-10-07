using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface ISliderService
    {
        IEnumerable<Slider> GetAll();
        Task<BusisnessResult> Create(string name, IFormFile image);
    }
}
