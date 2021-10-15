using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Market_Express.Domain.Services
{
    public class BaseService
    {
        protected bool IsValidImage(IFormFile image)
        {
            var validImageTypes = new[] {

                    "image/png",
                    "image/jpg",
                    "image/jpeg"
                };

            if (!validImageTypes.Any(x => x == image.ContentType))
            {
                return false;
            }

            return true;
        }
    }
}
