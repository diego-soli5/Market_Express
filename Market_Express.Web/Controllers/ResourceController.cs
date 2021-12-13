using Market_Express.Domain.Abstractions.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet("Img")]
        public async Task<IActionResult> GetImage([FromQuery(Name = "n")] string n)
        {
            if (string.IsNullOrWhiteSpace(n))
                return BadRequest();

            var oResult = await _resourceService.GetImage(n);

            if (oResult.Item1 == null && oResult.Item2 == null)
                return NotFound();

            return File(oResult.Item1, oResult.Item2);
        }

        [HttpGet("UserManual")]
        public IActionResult UserManual()
        {
            Stream oFileStream = _resourceService.GetUserManual();

            string contentType = "application/pdf";
            
            return File(oFileStream, contentType);
        }
    }
}
