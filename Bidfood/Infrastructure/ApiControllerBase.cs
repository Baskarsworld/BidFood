using Microsoft.AspNetCore.Mvc;

namespace Bidfood.Infrastructure
{
    [ApiController]
    public class ApiControllerBase: Controller
    {
        protected IActionResult CreateResponse(ApiResponse response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
