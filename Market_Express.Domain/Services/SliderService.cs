using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class SliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobStorageService _storageService;

        public SliderService(IUnitOfWork unitOfWork,
                             IAzureBlobStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task<BusisnessResult> Create()
        {
            return null;
        }
    }
}
