using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IClientValidations
    {
        bool ExistsClientCode();
        Client Client { set; }
    }
}
