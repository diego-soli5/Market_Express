using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Market_Express.Domain.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Slider> GetAllSliders()
        {
            return _unitOfWork.Slider.GetAllActive().ToList();
        }
    }
}
