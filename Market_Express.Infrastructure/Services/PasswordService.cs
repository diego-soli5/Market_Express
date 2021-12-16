using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Market_Express.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _passwordOptions;
     
        public PasswordService(IOptions<PasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
        }

        public bool Check(string sHashedPassword, string sPlainPassword)
        {
            var arrParts = sHashedPassword.Split('.');

            if (arrParts.Length != 3)
            {
                throw new FormatException("Unexpected hash format.");
            }

            int iIterations = Convert.ToInt32(arrParts[0]);
            byte[] btSalt = Convert.FromBase64String(arrParts[1]);
            byte[] btKey = Convert.FromBase64String(arrParts[2]);

            using (var oAlgorithm = new Rfc2898DeriveBytes(sPlainPassword, btSalt, iIterations, HashAlgorithmName.SHA512))
            {
                byte[] btKeyToCheck = oAlgorithm.GetBytes(_passwordOptions.KeySize);

                return btKeyToCheck.SequenceEqual(btKey);
            }
        }

        public string Hash(string sPlainPassword)
        {
            using (var oAlgorithm = new Rfc2898DeriveBytes(sPlainPassword, _passwordOptions.SaltSize, _passwordOptions.Iterations, HashAlgorithmName.SHA512))
            {
                string sKey = Convert.ToBase64String(oAlgorithm.GetBytes(_passwordOptions.KeySize));
                string sSalt = Convert.ToBase64String(oAlgorithm.Salt);

                return $"{_passwordOptions.Iterations}.{sSalt}.{sKey}";
            }
        }
    }
}
