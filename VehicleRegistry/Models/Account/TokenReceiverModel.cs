namespace VehicleRegistry.Client.Models.Account
{
    public class TokenRecieverModel
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
    }

}