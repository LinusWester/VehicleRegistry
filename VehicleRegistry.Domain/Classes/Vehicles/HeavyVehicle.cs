using System;
using System.Collections.Generic;
using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Domain.Classes.Vehicles
{
    public class HeavyVehicle : IVehicle
    {
        public HeavyVehicle(string regNumber, string model, string make, double vehicleWeight, DateTime firstDateInTraffic,
                       bool isRegistered, IService isServiceBooked, IList<IVehicleService> serviceHistory)
        {
            this.regNumber = regNumber;
            this.model = model;
            this.make = make;
            this.vehicleWeight = vehicleWeight;
            this.firstDateInTraffic = firstDateInTraffic;
            this.isRegistered = isRegistered;
            this.isServiceBooked = isServiceBooked;
            this.serviceHistory = serviceHistory;
            this.yearlyCost = 4500;
        }

        private string regNumber { get; set; }
        private string make { get; set; }
        private string model { get; set; }
        private double vehicleWeight { get; set; }
        private DateTime firstDateInTraffic { get; set; }
        private bool isRegistered { get; set; }
        private IService isServiceBooked { get; set; }
        private IList<IVehicleService> serviceHistory { get; set; }
        private double yearlyCost { get; set; }

        public string RegNumber => regNumber;
        public string Model => model;
        public string Make => make;
        public double VehicleWeight => vehicleWeight;
        public DateTime FirstDateInTraffic => firstDateInTraffic;
        public bool IsRegistered => isRegistered;
        public IService IsServiceBooked => isServiceBooked;
        public IList<IVehicleService> ServiceHistory => serviceHistory;
        public double YearlyCost => yearlyCost;

        public void BookNewService(IService service)
        {
            isServiceBooked = service;
        }
    }
}
