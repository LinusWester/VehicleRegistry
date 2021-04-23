using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Domain.Classes
{
    public class VehicleService : IVehicleService
    {
        public List<IVehicle> Search(List<IVehicle> vehicleList, string search)
        {
            if (string.IsNullOrEmpty(search))
                return vehicleList;

            List<IVehicle> newVehicleList = new List<IVehicle>();

            List<List<IVehicle>> Lists = new List<List<IVehicle>>(){
                new List<IVehicle>(),
                new List<IVehicle>(),
                new List<IVehicle>(),
            };

            Lists[0] = vehicleList.Where(o => Regex.IsMatch(o.RegNumber, search, RegexOptions.IgnoreCase)).ToList();
            Lists[1] = vehicleList.Where(o => Regex.IsMatch(o.Model, search, RegexOptions.IgnoreCase)).ToList();
            Lists[2] = vehicleList.Where(o => Regex.IsMatch(o.Make, search, RegexOptions.IgnoreCase)).ToList();

            newVehicleList = Lists[0].Union(Lists[1]).ToList();
            newVehicleList = newVehicleList.Union(Lists[2]).ToList();

            return newVehicleList;
        }

        public List<IVehicle> BookService(IService service, List<IVehicle> vehicles)
        {
            List<string> bookedServices = new List<string>();

            foreach (IVehicle vehicle in vehicles)
            {
                if (vehicle.IsServiceBooked != null)
                    bookedServices.Add(vehicle.RegNumber);

                vehicle.BookNewService(service);
            }
            if (bookedServices.Count > 0)
            {
                string BookedRegNumbers = "";
                for (var i = 0; 0 < bookedServices.Count; i++)
                {
                    BookedRegNumbers += bookedServices[i];
                    BookedRegNumbers += i + 1 == bookedServices.Count ? "" : ",";
                }
                throw new Exception($"There is already services booked for: {BookedRegNumbers}");
            }


            return vehicles;
        }

    }
}
