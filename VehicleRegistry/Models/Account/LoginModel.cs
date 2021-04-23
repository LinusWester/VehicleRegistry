using System;
using System.Text;
using VehicleRegistry.DTO.dto;

namespace VehicleRegistry.Client.Models.Account
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string grant_type { get; set; }

        public string GetTokenRequestString()
        {
            grant_type = "password";
            string request = $"username={Username}&password={Password}&grant_type=password";

            return request;
        }
    }
}