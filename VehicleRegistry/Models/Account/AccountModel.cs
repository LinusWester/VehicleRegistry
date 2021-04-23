using System;
using System.Text;
using VehicleRegistry.DTO.dto;

namespace VehicleRegistry.Client.Models.Account
{
    public class AccountModel
    {
        public string Username { get; set; }
        public bool Authorized { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public AccountRequestDto GetDto()
        {
            if (Password != ConfirmPassword)
                throw new Exception("Wrong password");

            AccountRequestDto request = new AccountRequestDto
            {
                Username = Username,
                Authorized = Authorized
            };
            var passwordByte = Encoding.UTF8.GetBytes(Password);
            request.Password = Convert.ToBase64String(passwordByte);

            return request;
        }


    }
}