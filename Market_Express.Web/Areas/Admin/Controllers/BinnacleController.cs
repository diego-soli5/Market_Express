using AutoMapper;
using Market_Express.Domain.Abstractions.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "BIN_USE_GEN")]
    public class BinnacleController : Controller
    {
        private readonly IBinnacleAccessService _binnacleAccessService;
        private readonly IMapper _mapper;

        private const string _ACC = "acc";
        private const string _MOV = "mov";

        public BinnacleController(IBinnacleAccessService binnacleAccessService,
                                  IMapper mapper)
        {
            _binnacleAccessService = binnacleAccessService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string tab)
        {
            if(tab != null)
            {
                if (tab.ToLower() == _ACC)
                    return await AccessIndexViewResult();
                else if (tab.ToLower() == _MOV)
                    return await MovementIndexViewResult();
            }

            return Redirect("/");
        }

        private async Task<IActionResult> AccessIndexViewResult()
        {
            return View("AccessIndex");
        }

        private async Task<IActionResult> MovementIndexViewResult()
        {
            return View("MovementIndex");
        }
    }
}
