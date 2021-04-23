using System;
using System.Collections.Generic;
using VehicleRegistry.Domain.Classes.Vehicles;
using VehicleRegistry.Domain.Interfaces;
using VehicleRegistry.DTO.dto;

namespace VehicleRegistry.Domain.Classes
{
    public class VehicleFactory
    {
        public IVehicle CreateVehicle(string regNumber, string model, string make, double vehicleWeight, DateTime firstDateInTraffic,
                       bool isRegistered, IService isServiceBooked, IList<IVehicleService> serviceHistory)
        {
            switch (vehicleWeight)
            {
                case double w when (w < 1800):
                    return new LightVehicle(regNumber, model, make, vehicleWeight, firstDateInTraffic,
                                                 isRegistered, isServiceBooked, serviceHistory);

                case double w when (w > 1800 && w < 2500):
                    return new MediumVehicle(regNumber, model, make, vehicleWeight, firstDateInTraffic,
                                                isRegistered, isServiceBooked, serviceHistory);

                default:
                    return new HeavyVehicle(regNumber, model, make, vehicleWeight, firstDateInTraffic,
                                                 isRegistered, isServiceBooked, serviceHistory);
            }
        }

        public IService CreateService(DateTime serviceDate, string serviceHistory)
        {
            return new Service(serviceDate, serviceHistory);
        }
    }
}
