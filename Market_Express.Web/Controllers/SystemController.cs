using AutoMapper;
using Market_Express.Application.DTOs.System;
using Market_Express.CrossCutting.Response;
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

        [HttpPost(nameof(ConnectionTest))]
        public IActionResult ConnectionTest()
        {
            if (!IsSyncAuthorized())
                return Unauthorized();

            return Ok(new SyncResponse { Success = true });
        }

        [HttpPost(nameof(SyncArticles))]
        public async Task<IActionResult> SyncArticles([FromBody] List<ArticuloSyncDTO> lstArticlesToSyncDTO)
        {
            if (!IsSyncAuthorized())
                return Unauthorized();

            var lstArticlesToSync = new List<Article>();

            lstArticlesToSyncDTO?.ForEach(art =>
            {
                lstArticlesToSync.Add(_mapper.Map<Article>(art));
            });

            var oResponse = await _systemService.SyncArticles(lstArticlesToSync);

            return Ok(oResponse);
        }

        [HttpPost(nameof(SyncClients))]
        public async Task<IActionResult> SyncClients([FromBody] List<ClienteSyncDTO> lstClientsToSyncDTO)
        {
            if (!IsSyncAuthorized())
                return Unauthorized();

            var lstClientsToSync = new List<Client>();

            lstClientsToSyncDTO?.ForEach(cli =>
            {
                lstClientsToSync.Add(_mapper.Map<Client>(cli));
            });

            var oResponse = await _systemService.SyncClients(lstClientsToSync);

            return Ok(oResponse);
        }

        #region UTILITY METHODS
        private bool IsSyncAuthorized()
        {
            if (!Request.Headers.ContainsKey("X-Sync-Authorization"))
                return false;

            if (!_authenticationService.CheckSyncAuthHeader(Request.Headers.First(h => h.Key == "X-Sync-Authorization").Value))
                return false;

            return true;
        }
        #endregion
    }
}
