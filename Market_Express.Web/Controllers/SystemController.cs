using Market_Express.Application.DTOs.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        public SystemController()
        {

        }

        [HttpPost(nameof(SyncArticulos))]
        public async Task<IActionResult> SyncArticulos([FromBody] List<ArticuloSyncDTO> articulos)
        {
            return Ok();
        }
    }
}
