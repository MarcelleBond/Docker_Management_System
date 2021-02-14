using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Night_Shadow.Models;
using Okta.Sdk;
using Okta.Sdk.Configuration;




namespace Night_Shadow.Controllers
{


    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        public async Task PostAsync([FromBody] Register reg)
        {
            var oktaClient = new OktaClient();
            var user = await oktaClient.Users.CreateUserAsync(new CreateUserWithPasswordOptions
            {
                Profile = new UserProfile
                {
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    Email = reg.Email,
                    Login = reg.Email
                },
                Password = reg.Password,
                Activate = true
            });

        }
    }
}