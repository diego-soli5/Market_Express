using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.Extensions.Options;

namespace Market_Express.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions _authOptions;

        public AuthenticationService(IOptions<AuthenticationOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }

        public bool CheckSyncAuthHeader(string authHeader)
        {
            return _authOptions.SyncAuthenticationSecret == authHeader;
        }
    }
}
