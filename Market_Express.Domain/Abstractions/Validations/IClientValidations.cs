using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IClientValidations
    {
        bool ExistsCodCliente();
        Client Cliente { set; }
    }
}
