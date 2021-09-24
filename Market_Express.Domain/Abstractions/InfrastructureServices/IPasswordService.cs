namespace Market_Express.Domain.Abstractions.InfrastructureServices
{
    public interface IPasswordService
    {
        bool Check(string hashedPassword, string plainPassword);
        string Hash(string plainPassword);
    }
}
