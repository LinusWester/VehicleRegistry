using System;

namespace VehicleRegistry.Domain.Interfaces
{
    public interface IService
    {
        DateTime ServiceDate { get; }
        string ServiceHistory { get; }

    }
}
