using Market_Express.Domain.Options;
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

        public bool CheckSyncAuthHeader(string sAuthHeader)
        {
            return _authOptions.SyncAuthenticationSecret?.Trim() == sAuthHeader?.Trim();
        }
    }
}
