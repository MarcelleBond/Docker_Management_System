using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Night_Shadow.Models;
using Okta.Sdk;




namespace Night_Shadow.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {

        private readonly ILogger<UserProfileController> _logger;
        public UserProfileController(ILogger<UserProfileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> findUserprofile()
        {
            var user = User.Claims.FirstOrDefault((x) => x.Type == "uid");
            OktaClient oktaClient = new OktaClient();
            IUser client = await oktaClient.Users.GetUserAsync(user.Value);
			return Ok(client.Profile);

        }

		[HttpPost]
		[Route("updatePassword")]
		public async Task<IActionResult> updatePassword([FromBody] Profile password)
		{
			Console.WriteLine("this is the new password: ");
			Console.WriteLine(password.Password);
			var user = User.Claims.FirstOrDefault((x) => x.Type == "uid");
            OktaClient oktaClient = new OktaClient();
            IUser client = await oktaClient.Users.GetUserAsync(user.Value);
			return Ok(client.Credentials.Password);	
		}

		[HttpPost]
		[Route("updateUsername")]
		public async Task<IActionResult> updateUsername()
		{

			return Ok("this is a post to update Username");	
		}

		[HttpPost]
		[Route("updateEmail")]
		public async Task<IActionResult> updateEmail()
		{
			return Ok("this is a post to update Email");	
		}
    }
}