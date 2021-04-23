using System;
using System.Collections.Generic;

namespace VehicleRegistry.Domain.Interfaces
{
    public interface IVehicle
    {
        string RegNumber { get; }
        string Model { get; }
        string Make { get; }
        double VehicleWeight { get; }
        DateTime FirstDateInTraffic { get; }
        bool IsRegistered { get; }
        IService IsServiceBooked { get; }
        IList<IVehicleService> ServiceHistory { get; }
        double YearlyCost { get; }

        void BookNewService(IService service);

    }
}
