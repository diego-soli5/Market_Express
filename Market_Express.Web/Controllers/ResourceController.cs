using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IAzureBlobStorageService _storageService;

        public ResourceController(IAzureBlobStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("Img")]
        public async Task<IActionResult> GetImage([FromQuery(Name = "n")]string n)
        {
            if (string.IsNullOrWhiteSpace(n))
                return BadRequest();

            var oResult = await _storageService.GetBlobAsync(n);

            if (oResult.Item1 == null && oResult.Item2 == null)
                return NotFound();

            return File(oResult.Item1, oResult.Item2);
        }

        public async Task<IActionResult> UserManual()
        {
            return Ok();
        }
    }
}
