using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Night_Shadow.Models;
using Okta.Sdk;
using System.Collections.Generic;

namespace Night_Shadow.Controllers
{
	// [Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class Test : ControllerBase
	{

		private readonly ILogger<Test> _logger;
		private readonly DB _db;
		public Test(ILogger<Test> logger)
		{
			_db = DB.GetInstance();
			_logger = logger;
		}

		[HttpGet]
		[Route("getUsers")]
		public string GetUsers(string ID)
		{
			var user = new ClientProfile(ID);
			return user.ToString();
		}

		[HttpPost]
		[Route("inserUsers")]
		public int insertUsers(ClientProfile profile)
		{
			int check = _db.Insert("Users", new Dictionary<string, object>()
			{
				["UserId"] = profile.UserId,
				["NickName"] = profile.NickName,
				["Email"] = profile.Email
			});
			return check;
		}

		[HttpPost]
		[Route("updateUser")]
		public Task<bool> UpdateUser(ClientProfile profile)
		{
			try
			{
				return profile.UpdateNickName(null);
			}
			catch (System.Exception)
			{
				throw;
			}
		}

		[HttpPost]
		[Route("deleteUser")]
		public int DeleteUser(ClientProfile profile)
		{
			return _db.Delete("Users", "=", new Dictionary<string, object>() { ["NickName"] = profile.NickName });
		}
	}
}