using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.EntityValidations
{
    public class ClienteValidations : IClienteValidations
    {
        private Cliente _cliente;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteValidations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Cliente Cliente
        {
            private get { return _cliente; }
            set { _cliente = value; }
        }

        public bool ExistsCodCliente()
        {
            return _unitOfWork.Cliente.GetFirstOrDefault(x => x.CodCliente == Cliente.CodCliente) != null;
        }
    }
}
