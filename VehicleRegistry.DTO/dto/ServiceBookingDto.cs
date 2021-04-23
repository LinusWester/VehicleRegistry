using System;
using System.Collections.Generic;

namespace VehicleRegistry.DTO.dto
{
    public class ServiceBookingDto
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceHistory { get; set; }
        public List<string> RegNumbers { get; set; } = new List<string>();
    }
}
