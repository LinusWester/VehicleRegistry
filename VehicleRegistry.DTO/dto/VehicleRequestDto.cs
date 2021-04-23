using System;
using System.Collections.Generic;

namespace VehicleRegistry.DTO.dto
{
    public class VehicleRequestDto
    {
        public string RegNumber { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public double VehicleWeight { get; set; }
        public DateTime FirstDateInTraffic { get; set; }
        public bool IsRegistered { get; set; }
        public VehicleServiceDto IsServiceBooked { get; set; }
        public List<VehicleServiceDto> ServiceHistory { get; set; } = new List<VehicleServiceDto>();
    }

    public class VehicleServiceDto
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceHistory { get; set; }
        public bool IsServiceBooked { get; set; }
    }
}
