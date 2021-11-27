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
using Rotativa.AspNetCore;
using Market_Express.Domain.QueryFilter.BinnacleMovement;
using Market_Express.Application.DTOs.BinnacleMovement;
using Market_Express.Web.ViewModels.BinnacleMovement;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "BIN_USE_GEN")]
    public class BinnacleController : Controller
    {
        private readonly IBinnacleAccessService _binnacleAccessService;
        private readonly IBinnacleMovementService _binnacleMovementService;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public BinnacleController(IBinnacleAccessService binnacleAccessService,
                                  IBinnacleMovementService binnacleMovementService,
                                  IAppUserService appUserService,
                                  IMapper mapper)
        {
            _binnacleAccessService = binnacleAccessService;
            _binnacleMovementService = binnacleMovementService;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Movement(BinnacleMovementQueryFilter filters)
        {
            BinnacleMovementIndexViewModel oViewModel = new();

            var tplBinnacleMovementDTO = await GetBinnacleMovementDTOList(filters);

            oViewModel.BinnacleMovements = tplBinnacleMovementDTO.Item1;
            oViewModel.Metadata = tplBinnacleMovementDTO.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        public async Task<IActionResult> MovementReport(BinnacleMovementQueryFilter filters)
        {
            MovementReportViewModel oViewModel = new();

            oViewModel.Filters = filters;
            oViewModel.BinnacleMovements = (await _binnacleMovementService.GetAllForReport(filters))
                                                                          .Select(b => _mapper.Map<BinnacleMovementDTO>(b))
                                                                          .ToList();

            return new ViewAsPdf("MovementReport", oViewModel);
        }

        [HttpGet]
        public IActionResult Access(BinnacleAccessQueryFilter filters)
        {
            BinnacleAccessIndexViewModel oViewModel = new();

            var tplBinnacleAccessDTO = GetBinnacleAccessDTOList(filters);

            oViewModel.BinnacleAccesses = tplBinnacleAccessDTO.Item1;
            oViewModel.Metadata = tplBinnacleAccessDTO.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        [HttpGet]
        public IActionResult AccessReport(BinnacleAccessQueryFilter filters)
        {
            AccessReportViewModel oViewModel = new();

            oViewModel.Filters = filters;
            oViewModel.BinnaclesAccesses = _binnacleAccessService.GetAllForReport(filters)
                                                             .Select(b => _mapper.Map<BinnacleAccessDTO>(b))
                                                             .ToList();

            return new ViewAsPdf("AccessReport", oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Binnacle/Access/GetTable")]
        public IActionResult GetAccessTable(BinnacleAccessQueryFilter filters)
        {
            BinnacleAccessIndexViewModel oViewModel = new();

            var tplBinnacleAccessDTO = GetBinnacleAccessDTOList(filters);

            oViewModel.BinnacleAccesses = tplBinnacleAccessDTO.Item1;
            oViewModel.Metadata = tplBinnacleAccessDTO.Item2;
            oViewModel.Filters = filters;

            return PartialView("_BinnacleAccessTablePartial", oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Binnacle/Movement/GetTable")]
        public async Task<IActionResult> GetMovementTable(BinnacleMovementQueryFilter filters)
        {
            BinnacleMovementIndexViewModel oViewModel = new();

            var tplBinnacleMovementDTO = await GetBinnacleMovementDTOList(filters);

            oViewModel.BinnacleMovements = tplBinnacleMovementDTO.Item1;
            oViewModel.Metadata = tplBinnacleMovementDTO.Item2;
            oViewModel.Filters = filters;

            return PartialView("_BinnacleMovementTablePartial", oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Binnacle/Access/SearchUser")]
        public IActionResult SearchUser(string query, bool onlyAdmin)
        {
            var lstResult = _appUserService.SearchNames(query, onlyAdmin);

            return Ok(lstResult.Select(s => new { Name = s }));
        }

        #region UTILTY METHODS
        private async Task<(List<BinnacleMovementDTO>, Metadata)> GetBinnacleMovementDTOList(BinnacleMovementQueryFilter filters)
        {
            var pagedList = await _binnacleMovementService.GetAllPaginated(filters);

            var meta = Metadata.Create(pagedList);

            return (pagedList.Select(b => _mapper.Map<BinnacleMovementDTO>(b)).ToList(), meta);
        }

        private (List<BinnacleAccessDTO>, Metadata) GetBinnacleAccessDTOList(BinnacleAccessQueryFilter filters)
        {
            var pagedList = _binnacleAccessService.GetAllPaginated(filters);

            var meta = Metadata.Create(pagedList);

            return (pagedList.Select(b => _mapper.Map<BinnacleAccessDTO>(b)).ToList(), meta);
        }
        #endregion
    }
}
