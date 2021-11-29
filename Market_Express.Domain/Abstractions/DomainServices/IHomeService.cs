using Market_Express.Domain.Entities;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IHomeService
    {
        List<Slider> GetAllSliders();
        BusisnessResult SendMessageToDeveloper(string name, string phone, string email, string message);
    }
}
