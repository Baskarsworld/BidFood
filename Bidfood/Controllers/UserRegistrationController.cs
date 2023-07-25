using Bidfood.Infrastructure;
using Bidfood.Models;
using Bidfood.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bidfood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : ApiControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public UserRegistrationController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost]
        public IActionResult Post(UserDetail userDetail)
        {
            var response = _userRegistrationService.UserRegistration(userDetail);
            return CreateResponse(response); 
        }       
    }
}