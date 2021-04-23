using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleRegistry.DTO.dto;

namespace VehicleRegistry.Client.Models.VehicleRegistry
{
    public class ServiceBookingModel
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceHistory { get; set; }
        public string FirstRegNumber { get; set; }
        public string SecondRegNumber { get; set; }
        public string ThirdRegNumber { get; set; }

        public ServiceBookingDto GetDto()
        {
            ServiceBookingDto request = new ServiceBookingDto
            {
                ServiceDate = ServiceDate,
                ServiceHistory = ServiceHistory
            };
            if (FirstRegNumber != null)
                request.RegNumbers.Add(FirstRegNumber);

            if (SecondRegNumber != null)
                request.RegNumbers.Add(SecondRegNumber);

            if (ThirdRegNumber != null)
                request.RegNumbers.Add(ThirdRegNumber);

            return request;
        }
    }
}