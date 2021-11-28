using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IAppUserValidations
    {
        bool ExistsIdentification();
        bool ExistsEmail();
        AppUser AppUser { set; }
    }
}
