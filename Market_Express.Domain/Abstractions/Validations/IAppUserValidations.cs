using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.Validations
{
    public interface IAppUserValidations
    {
        bool ExistsCedula();
        bool ExistsEmail();
        AppUser Usuario { set; }
    }
}
