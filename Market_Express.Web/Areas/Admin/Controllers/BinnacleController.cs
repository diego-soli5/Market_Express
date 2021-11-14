using AutoMapper;
using Market_Express.Application.DTOs.BinnacleAccess;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.BinnacleAccess;
using Market_Express.Web.ViewModels.BinnacleAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "BIN_USE_GEN")]
    public class BinnacleController : Controller
    {
        private readonly IBinnacleAccessService _binnacleAccessService;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        private const string _ACC = "acc";
        private const string _MOV = "mov";

        public BinnacleController(IBinnacleAccessService binnacleAccessService,
                                  IAppUserService appUserService,
                                  IMapper mapper)
        {
            _binnacleAccessService = binnacleAccessService;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Movement()
        {
            return View("MovementIndex");
        }

        [HttpGet]
        public IActionResult Access(BinnacleAccessQueryFilter filters)
        {
            BinnacleAccessIndexViewModel oViewModel = new();

            filters.StartDate = DateTimeUtility.NowCostaRica;

            var tplBinnacleAccessDTO = GetBinnacleAccessDTOList(filters);

            oViewModel.BinnacleAccesses = tplBinnacleAccessDTO.Item1;
            oViewModel.Metadata = tplBinnacleAccessDTO.Item2;
            oViewModel.Filters = filters;

            return View("AccessIndex", oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Binnacle/Access/GetTable")]
        public IActionResult GetTable(BinnacleAccessQueryFilter filters)
        {
            BinnacleAccessIndexViewModel oViewModel = new();

            var tplBinnacleAccessDTO = GetBinnacleAccessDTOList(filters);

            oViewModel.BinnacleAccesses = tplBinnacleAccessDTO.Item1;
            oViewModel.Metadata = tplBinnacleAccessDTO.Item2;
            oViewModel.Filters = filters;

            return PartialView("_BinnacleAccessTablePartial", oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Binnacle/Access/SearchUser")]
        public IActionResult SearchUser(string query)
        {
            var lstResult = _appUserService.SearchNames(query);

            return Ok(lstResult.Select(s => new { Name = s }));
        }

        #region UTILTY METHODS
        private (List<BinnacleAccessDTO>, Metadata) GetBinnacleAccessDTOList(BinnacleAccessQueryFilter filters)
        {
            var pagedList = _binnacleAccessService.GetAll(filters);

            var meta = Metadata.Create(pagedList);

            return (pagedList.Select(b => _mapper.Map<BinnacleAccessDTO>(b)).ToList(), meta);
        }
        #endregion
    }
}
