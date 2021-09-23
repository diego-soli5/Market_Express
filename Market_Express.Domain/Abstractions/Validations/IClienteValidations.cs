using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IClienteValidations
    {
        bool ExistsCodCliente();
        Cliente Cliente { set; }
    }
}
