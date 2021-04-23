using System;

namespace VehicleRegistry.Client.Models.VehicleRegistry
{
    public class ServiceModel
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceHistory { get; set; }

        public ServiceModel(DateTime serviceDate, string serviceHistory)
        {
            this.ServiceDate = serviceDate;
            this.ServiceHistory = serviceHistory;
        }
    }
}