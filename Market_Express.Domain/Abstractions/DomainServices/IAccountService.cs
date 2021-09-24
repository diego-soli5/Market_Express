using Market_Express.Domain.Entities;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAccountService
    {
        BusisnessResult TryAuthenticate(Usuario usuarioRequest);
    }
}
