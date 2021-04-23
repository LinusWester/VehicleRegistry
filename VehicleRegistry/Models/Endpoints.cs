using System.Configuration;


namespace VehicleRegistry.Client.Models
{
    public class EndPoints
    {
        string HostName => ConfigurationManager.AppSettings["hostname"];

        public string BookService => HostName + "api/bookservice";
        public string CompleteService => HostName + "api/completeservice";
        public string CreateAccount => HostName + "api/createaccount";
        public string CreateLogin => HostName + "api/login";
        public string CreateVehicle => HostName + "api/Createvehicle";
        public string GetToken => HostName + "token";
        public string GetVehicle => HostName + "api/getvehicle";
        public string GetVehicleList => HostName + "api/getvehiclelist";
        public string RemoveVehicle => HostName + "api/removevehicle";
        public string UpdateVehicle => HostName + "api/updatevehicle";
    }
}