using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market_Express.Domain.Services
{
    public class HomeService : BaseService, IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusisnessMailService _mailService;

        public HomeService(IUnitOfWork unitOfWork,
                           IBusisnessMailService mailService,
                           IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public List<Slider> GetAllSliders()
        {
            return _unitOfWork.Slider.GetAllActive().ToList();
        }

        public BusisnessResult SendMessageToDeveloper(string name, string phone, string email, string message)
        {
            BusisnessResult oResult = new();

            string sSubject = "Te han enviado un mensaje - ME";
            var sBody = @$"
                            <table>
                                <tr>
                                     <td><b>Nombre:</b> {name}</td>
                                </tr>
                                <tr>
                                     <td><b>Teléfono:</b> {phone}</td>
                                </tr>
                                <tr>
                                     <td><b>Correo Electrónico:</b> {email}</td>
                                </tr>
                                <tr>
                                     <td><b>Mensaje:</b> {message}</td>
                                </tr>
                            </table>";
            var sTo = new string[] { "1diego321@gmail.com", "diego.solis@promerica.fi.cr" };

            _mailService.SendMail(sSubject, sBody, sTo);

            oResult.Message = "El mensaje ha sido enviado!";

            oResult.Success = true;

            return oResult;
        }
    }
}
