using System.Collections.Generic;
using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        void CreateVehicle(IVehicle vehicle);
        List<IVehicle> GetAllVehicles();
        IVehicle GetVehicle(string regNumber);
        void UpdateVehicle(IVehicle vehicle);
        void DeleteVehicle(string regNumber);
        void CompleteService(string regNumber);
        void BookService(IVehicle vehicle);
    }
}
