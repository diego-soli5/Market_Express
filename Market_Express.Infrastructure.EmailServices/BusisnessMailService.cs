using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.Extensions.Options;

namespace Market_Express.Infrastructure.EmailServices
{
    public class BusisnessMailService : MasterMailServer, IBusisnessMailService
    {
        public BusisnessMailService(IOptions<EmailServicesOptions> options)
            : base(options)
        { }

    }
}
