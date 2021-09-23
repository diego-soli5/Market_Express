namespace Market_Express.Domain.Abstractions.InfrastructureServices
{
    public interface IAuthenticationService
    {
        bool IsSyncAuthorized(string authHeader);
    }
}