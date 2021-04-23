using System;
using System.Collections.Generic;

namespace VehicleRegistry.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<IVehicle> Search(List<IVehicle> vehicleList, string search);
        List<IVehicle> BookService(IService service, List<IVehicle> vehicles);
    }
}
