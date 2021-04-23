using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRegistry.Domain.Classes;
using VehicleRegistry.Domain.Interfaces;
using VehicleRegistry.Repository.Interfaces;

namespace VehicleRegistry.Repository.Classes
{
    public class AzureRepository : IVehicleRepository, IAccountRepository
    {
        private readonly AzureVehicleDatabaseDataContext dataContext;

        public AzureRepository()
        {
            dataContext = new AzureVehicleDatabaseDataContext();
        }

        private string AddVehicle(IVehicle vehicle)
        {
            var newVehicle = new Vehicle
            {
                RegNumber = vehicle.RegNumber,
                Model = vehicle.Model,
                Make = vehicle.Make,
                VehicleType = vehicle.GetType().Name,
                VehicleWeight = vehicle.VehicleWeight,
                FirstDateInTraffic = vehicle.FirstDateInTraffic,
                IsRegistered = vehicle.IsRegistered,
            };
            dataContext.Vehicles.InsertOnSubmit(newVehicle);
            dataContext.SubmitChanges();

            return newVehicle.RegNumber;
        }

        private int AddService(string regNumber, VehicleService service)
        {
            var newVehicleService = new VehicleService
            {
                RegNumber = regNumber,
                ServiceDate = service.ServiceDate,
                ServiceHistory = service.ServiceHistory,
                IsServiceBooked = true
            };
            dataContext.VehicleServices.InsertOnSubmit(newVehicleService);
            dataContext.SubmitChanges();

            return newVehicleService.Id;
        }
        public void BookService(IVehicle vehicle)
        {
            AddService(vehicle.RegNumber, (VehicleService)vehicle.IsServiceBooked);
        }

        public void CreateVehicle(IVehicle vehicle)
        {
            foreach (var entity in dataContext.Vehicles)
            {
                if (entity.RegNumber == vehicle.RegNumber)
                    throw new Exception("Vehicle with that registration number already exists in the database");
            }

            var regNumber = AddVehicle(vehicle);

            if (vehicle.IsServiceBooked != null)
                AddService(regNumber, (VehicleService)vehicle.IsServiceBooked);

            foreach (IVehicleService service in vehicle.ServiceHistory)
                AddService(regNumber, (VehicleService)service);
        }
        public void CompleteService(string regNumber)
        {
            var dbVehicleServiceList = dataContext.VehicleServices.Where(o => o.RegNumber == regNumber);
            var serviceIdList = dbVehicleServiceList.Select(o => o.Id);
            foreach (var id in serviceIdList)
            {
                var service = dataContext.VehicleServices.Where(o => o.Id == id).Single();
                if (service.IsServiceBooked == false)
                    service.IsServiceBooked = true;
            }
        }

        public void DeleteVehicle(string regNumber)
        {
            var dbVehicle = dataContext.Vehicles.Where(o => o.RegNumber == regNumber).FirstOrDefault();
            if (dbVehicle == null)
                throw new Exception("No vehicle with that Registration number found in the database");

            dataContext.Vehicles.DeleteOnSubmit(dbVehicle);

            var dbVehicleServiceList = dataContext.VehicleServices.Where(o => o.RegNumber == regNumber);
            foreach (var vehicleService in dbVehicleServiceList)
            {
                dataContext.VehicleServices.DeleteOnSubmit(vehicleService);
            }

            var serviceIdList = dbVehicleServiceList.Select(o => o.Id);
            foreach (var id in serviceIdList)
            {
                var service = dataContext.VehicleServices.Where(o => o.Id == id).Single();
                dataContext.VehicleServices.DeleteOnSubmit(service);
            }

            dataContext.SubmitChanges();
        }

        public void RegisterAccount(IAccount account)
        {
            foreach (var entity in dataContext.Accounts)
            {
                if (entity.Username == account.Username)
                    throw new Exception("Username already in use");
            }
            var newAccount = new Account
            {
                Username = account.Username,
                Authorized = account.Authorized,
                UserPassword = account.Password
            };

            dataContext.Accounts.InsertOnSubmit(newAccount);
            dataContext.SubmitChanges();

        }
        public void UpdateVehicle(IVehicle vehicle)
        {
            var dbVehicle = dataContext.Vehicles.Where(o => o.RegNumber == vehicle.RegNumber).FirstOrDefault();
            if (dbVehicle == null)
                throw new Exception("Vehicle not found in database");

            dbVehicle.Model = vehicle.Model;
            dbVehicle.Make = vehicle.Make;
            dbVehicle.VehicleWeight = vehicle.VehicleWeight;
            dbVehicle.FirstDateInTraffic = vehicle.FirstDateInTraffic;
            dbVehicle.IsRegistered = vehicle.IsRegistered;
        }

        public List<IVehicle> GetAllVehicles()
        {
            List<IVehicle> vehicleList = new List<IVehicle>();

            foreach (var entity in dataContext.Vehicles)
            {
                vehicleList.Add(GetVehicle(entity.RegNumber));
            }

            return vehicleList;
        }

        public IVehicle GetVehicle(string regNumber)
        {
            VehicleFactory factory = new VehicleFactory();
            var dbVehicle = dataContext.Vehicles.Where(o => o.RegNumber == regNumber).FirstOrDefault();
            if (dbVehicle == null)
                throw new Exception("No vehicle with that Registration number found in the database");

            var dbVehicleServiceList = dataContext.VehicleServices.Where(o => o.RegNumber == regNumber);
            var serviceIdList = dbVehicleServiceList.Select(o => o.Id);
            List<IVehicleService> serviceList = new List<IVehicleService>();
            IService isServiceBooked = null;
            foreach (var id in serviceIdList)
            {
                var dbService = dataContext.VehicleServices.Where(o => o.Id == id).Single();
                if (dbService.IsServiceBooked == false)
                {
                    isServiceBooked = factory.CreateService(dbService.ServiceDate, dbService.ServiceHistory);
                }
                serviceList.Add((IVehicleService)factory.CreateService(dbService.ServiceDate, dbService.ServiceHistory));
            }

            IVehicle vehicle = factory.CreateVehicle(dbVehicle.RegNumber, dbVehicle.Model,
                                                     dbVehicle.Make, dbVehicle.VehicleWeight,
                                                     (DateTime)dbVehicle.FirstDateInTraffic,
                                                     dbVehicle.IsRegistered, isServiceBooked, serviceList);
            return vehicle;
        }

        public List<IAccount> GetAllAccounts()
        {
            List<IAccount> accountList = new List<IAccount>();
            foreach (var entity in dataContext.Accounts)
            {
                accountList.Add(GetAccount(entity.Username));
            }

            return accountList;
        }

        public IAccount GetAccount(string username)
        {
            var dbAccount = dataContext.Accounts.Where(o => o.Username == username).FirstOrDefault();
            if (dbAccount == null)
                throw new Exception("Account not found in database");

            IAccount account = new Domain.Classes.Account(dbAccount.Username,
                                                          dbAccount.Authorized,
                                                          dbAccount.UserPassword);
            return account;
        }
    }
}
