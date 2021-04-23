namespace VehicleRegistry.DTO.dto
{
    public class AccountRequestDto
    {
        public string Username { get; set; }
        public bool Authorized { get; set; }
        public string Password { get; set; }
    }
}
