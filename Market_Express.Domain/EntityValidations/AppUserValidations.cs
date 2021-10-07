using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.EntityValidations
{
    public class AppUserValidations : IAppUserValidations
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppUser _usuario;

        public AppUserValidations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AppUser Usuario
        {
            private get { return _usuario; }
            set { _usuario = value; }
        }

        public bool ExistsIdentification()
        {
            return _unitOfWork.AppUser
                .GetFirstOrDefault(x => x.Identification == Usuario.Identification) != null;
        }

        public bool OwnsExistingCedula()
        {
            return _unitOfWork.AppUser
                .GetFirstOrDefault(x => x.Identification == Usuario.Identification && x.Id == Usuario.Id) != null;
        }

        public bool ExistsEmail()
        {
            return _unitOfWork.AppUser
                .GetFirstOrDefault(x => x.Email == Usuario.Email) != null;
        }
    }
}
