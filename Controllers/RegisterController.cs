using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Night_Shadow.Models;
using Okta.Sdk;


namespace Night_Shadow.Controllers
{


    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Register reg)
        {
            var oktaClient = new OktaClient();
            try
            {
                IUser user = await oktaClient.Users.CreateUserAsync(
                new CreateUserWithPasswordOptions
                {
                    Profile = new UserProfile
                    {
                        FirstName = reg.FirstName,
                        LastName = reg.LastName,
                        Email = reg.Email,
                        NickName = reg.NickName,
                        Login = reg.Email
                    },
                    Password = reg.Password,
                    Activate = true
                });
                return Created("/profile",user.Profile);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict(ex.Message);
            }
        }
    }
}