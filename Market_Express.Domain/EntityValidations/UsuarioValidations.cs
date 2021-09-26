using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.EntityValidations
{
    public class UsuarioValidations : IUsuarioValidations
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppUser _usuario;

        public UsuarioValidations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AppUser Usuario
        {
            private get { return _usuario; }
            set { _usuario = value; }
        }

        public bool ExistsCedula()
        {
            return _unitOfWork.Usuario
                .GetFirstOrDefault(x => x.Cedula == Usuario.Cedula) != null;
        }

        public bool OwnsExistingCedula()
        {
            return _unitOfWork.Usuario
                .GetFirstOrDefault(x => x.Cedula == Usuario.Cedula && x.Id == Usuario.Id) != null;
        }

        public bool ExistsEmail()
        {
            return _unitOfWork.Usuario
                .GetFirstOrDefault(x => x.Email == Usuario.Email) != null;
        }
    }
}
