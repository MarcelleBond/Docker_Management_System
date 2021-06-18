using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Night_Shadow.Models;
using Okta.Sdk;
using AutoMapper;

namespace Night_Shadow.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UserProfileController : ControllerBase
	{

		private readonly ILogger<UserProfileController> _logger;
		private readonly OktaClient _oktaClient;
		private readonly IMapper _mapper;
		public UserProfileController(ILogger<UserProfileController> logger, IMapper mapper)
		{
			_logger = logger;
			_oktaClient = new OktaClient();
			_mapper = mapper;
		}

		[HttpPost]
		[Route("createUser")]
		public async Task<IActionResult> CreateUser([FromBody] ClientProfile pro)
		{
			var oktaClient = new OktaClient();
			try
			{
				IUser user = await oktaClient.Users.CreateUserAsync(
				new CreateUserWithPasswordOptions
				{
					Profile = new UserProfile
					{
						Email = pro.Email,
						NickName = pro.NickName,
						Login = pro.Email
					},
					Password = pro.Password,
					Activate = true
				});
				var result = await pro.CreateUser();
				return Created("/profile", user.Profile);
			}
			catch (OktaApiException error)
			{
				Console.WriteLine(error.Message);
				return StatusCode(error.StatusCode, error.Message);
			}
		}

		[HttpGet]
		[Route("getUserProfile")]
		public async Task<IActionResult> getUserProfile()
		{
			var id = User.Claims.FirstOrDefault((x) => x.Type == "uid");
			IUser client = await _oktaClient.Users.GetUserAsync(id.Value);
			ClientProfile user = _mapper.Map<ClientProfile>(client.Profile);
			return Ok(user);
		}

		[HttpPost]
		[Route("updatePassword")]
		public async Task<IActionResult> updatePassword([FromBody] ClientProfile pro)
		{
			try
			{
				var user = User.Claims.FirstOrDefault((x) => x.Type == "uid");
				await pro.UpdatePassword(await _oktaClient.Users.GetUserAsync(user.Value));
				return StatusCode(200);
			}
			catch (OktaApiException error)
			{
				return StatusCode(error.StatusCode, error.Message);
			}

		}

		[HttpPost]
		[Route("updateUsername")]
		public async Task<IActionResult> updateNickName([FromBody] ClientProfile pro)
		{
			try
			{
				var user = User.Claims.FirstOrDefault((x) => x.Type == "uid");
				await pro.UpdateNickName(await _oktaClient.Users.GetUserAsync(user.Value));
				return StatusCode(200);
			}
			catch (OktaApiException error)
			{
				return StatusCode(error.StatusCode, error.Message);
			}
		}

		[HttpPost]
		[Route("updateEmail")]
		public async Task<IActionResult> updateEmail([FromBody] ClientProfile pro)
		{
			try
			{
				var user = User.Claims.FirstOrDefault((x) => x.Type == "uid");
				await pro.UpdateEmail(await _oktaClient.Users.GetUserAsync(user.Value));
				return StatusCode(200);
			}
			catch (OktaApiException error)
			{
				return StatusCode(error.StatusCode, error.Message);
			}
		}
		
	}
}