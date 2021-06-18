using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Okta.Sdk;
using Night_Shadow.Helpers;

namespace Night_Shadow.Models
{
    public class ClientProfile
    {
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        private DB _db;

        public ClientProfile()
        {
            _db = DB.GetInstance();
        }

        public ClientProfile(string ID)
        {
            _db = DB.GetInstance();
            this.Copy(_db.Select<ClientProfile>("Users", "=", new Dictionary<string, object>() { ["UserId"] = ID })[0]);
        }
        public async Task<bool> UpdatePassword(IUser user)
        {
            user.Credentials.Password.Value = Password;
            user = await user.UpdateAsync();
            return true;
        }
        public async Task<bool> UpdateEmail(IUser user)
        {
            user.Profile.Email = this.Email;
            user = await user.UpdateAsync();
            var rows = _db.Update("Users", new Dictionary<string, object>() { ["Email"] = this.Email }, "=", new Dictionary<string, object>() { ["UserId"] = this.UserId });
            return true;
        }
        public async Task<bool> UpdateNickName(IUser user)
        {
            user.Profile.NickName = this.NickName;
            user = await user.UpdateAsync();
            var rows = _db.Update("Users", new Dictionary<string, object>() { ["NickName"] = this.NickName }, "=", new Dictionary<string, object>() { ["UserId"] = this.UserId });
            return rows > 0 ? true : false;
        }

        public async Task<bool> CreateUser()
        {
            return true;
        }
    }
}