using System;
using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Domain.Classes
{
    public class Service : IService
    {
        public Service(DateTime serviceDate, string serviceHistory)
        {
            this.serviceDate = serviceDate;
            this.serviceHistory = serviceHistory;
        }

        private DateTime serviceDate { get; set; }
        private string serviceHistory { get; set; }

        public DateTime ServiceDate => serviceDate;
        public string ServiceHistory => serviceHistory;
    }
}
