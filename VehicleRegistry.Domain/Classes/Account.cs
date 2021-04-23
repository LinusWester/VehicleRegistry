using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Domain.Classes
{
    public class Account : IAccount
    {
        public Account(string username, bool authorized, string password)
        {
            this.username = username;
            this.authorized = authorized;
            this.password = password;
        }
        private string username { get; set; }
        private bool authorized { get; set; }
        private string password { get; set; }

        public string Username => username;
        public bool Authorized => authorized;
        public string Password => password;
    }
}
