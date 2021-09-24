namespace Market_Express.Domain.Abstractions.InfrastructureServices
{
    public interface IAuthenticationService
    {
        bool CheckSyncAuthHeader(string authHeader);
    }
}