using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.EntityValidations
{
    public class ClientValidations : IClientValidations
    {
        private Client _cliente;
        private readonly IUnitOfWork _unitOfWork;

        public ClientValidations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Client Cliente
        {
            private get { return _cliente; }
            set { _cliente = value; }
        }

        public bool ExistsCodCliente()
        {
            return _unitOfWork.Client.GetFirstOrDefault(x => x.ClientCode == Cliente.ClientCode) != null;
        }
    }
}
