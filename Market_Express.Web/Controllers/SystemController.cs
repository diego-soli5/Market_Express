using AutoMapper;
using Market_Express.Application.DTOs.System;
using Market_Express.Domain.Abstractions.ApplicationServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISystemService _systemService;
        private readonly IAuthenticationService _authenticationService;

        public SystemController(IMapper mapper, ISystemService systemService, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _systemService = systemService;
            _authenticationService = authenticationService;
        }

        [HttpPost(nameof(SyncArticles))]
        public async Task<IActionResult> SyncArticles([FromBody] List<ArticuloSyncDTO> lstArticulosToSyncDTO)
        {
            if (IsSyncAuthorized())
                return Unauthorized();

            var lstArticulosToSync = new List<InventarioArticulo>();

            lstArticulosToSyncDTO?.ForEach(art =>
            {
                lstArticulosToSync.Add(_mapper.Map<InventarioArticulo>(art));
            });

            var response = await _systemService.SyncArticles(lstArticulosToSync);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SyncClients([FromBody] List<ClienteSyncDTO> lstClientToSyncDTO)
        {
            if (IsSyncAuthorized())
                return Unauthorized();

            var lstClientsToSync = new List<Cliente>();

            lstClientToSyncDTO?.ForEach(cli =>
            {
                lstClientsToSync.Add(_mapper.Map<Cliente>(cli));
            });

            var response = await _systemService.SyncClients(lstClientsToSync);

            return Ok(response);
        }

        #region UTILITY METHODS
        private bool IsSyncAuthorized()
        {
            if (!Request.Headers.ContainsKey("X-Sync-Authorization"))
                return false;

            if (!_authenticationService.IsSyncAuthorized(Request.Headers.First(h => h.Key == "X-Sync-Authorization").ToString()))
                return false;

            return true;
        }
        #endregion
    }
}
