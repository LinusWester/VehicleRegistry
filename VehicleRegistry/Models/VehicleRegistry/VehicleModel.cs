using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleRegistry.Domain.Classes;
using VehicleRegistry.DTO.dto;

namespace VehicleRegistry.Models.Vehicle
{
    public class VehicleModel
    {
        public string RegNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public double VehicleWeight { get; set; }
        public DateTime FirstDateInTraffic { get; set; }
        public bool IsRegistered { get; set; }
        public Service IsServiceBooked { get; set; }
        public List<Service> ServiceHistory { get; set; }
        public string VehicleType { get; set; }

        public VehicleRequestDto GetDto()
        {
            VehicleRequestDto request = new VehicleRequestDto
            {
                RegNumber = RegNumber,
                Model = Model,
                Make = Make,
                VehicleWeight = VehicleWeight,
                FirstDateInTraffic = FirstDateInTraffic,
                IsRegistered = IsRegistered
            };

            if (IsServiceBooked != null)
                request.IsServiceBooked = GetServiceForDto(IsServiceBooked);

            if (ServiceHistory != null)
                foreach (Service service in ServiceHistory)
                    request.ServiceHistory.Add(GetServiceForDto(service));

            return request;
        }

        private VehicleServiceDto GetServiceForDto(Service Vehicleservice)
        {
            VehicleServiceDto service = new VehicleServiceDto
            {
                ServiceDate = Vehicleservice.ServiceDate,
                ServiceHistory = Vehicleservice.ServiceHistory
            };

            return service;
        }

        public string GetVehicleType()
        {
            switch (VehicleWeight)
            {
                case double w when(w < 1800):
                    return "LightVehicle";

                case double w when (w > 1800 && w < 2500):
                    return "MediumVehicle";

                default:
                    return "HeavyVehicle";
            }
        }
    }
}