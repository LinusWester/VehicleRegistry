using System.Collections.Generic;
using System.Web.Http;
using VehicleRegistry.DTO.dto;
using VehicleRegistry.Domain.Classes;
using VehicleRegistry.Domain.Interfaces;
using VehicleRegistry.Repository.Interfaces;

namespace VehicleRegistry.API.Controllers
{
   // [Authorize]
    public class VehicleController : ApiController
    {
        private readonly IVehicleService vehicleService;
        private readonly IVehicleRepository vehicleRepository;

        public VehicleController(IVehicleService vehicleService, IVehicleRepository vehicleRepository)
        {
            this.vehicleService = vehicleService;
            this.vehicleRepository = vehicleRepository;
        }

        private IVehicle CreateVehicleFromDto(VehicleRequestDto vehicleRequestDto)
        {
            var factory = new VehicleFactory();
            IList<IService> vehicleServices = new List<IService>();

            foreach (var service in vehicleRequestDto.ServiceHistory)
                vehicleServices.Add(factory.CreateService(service.ServiceDate, service.ServiceHistory));

            IService isServiceBooked = null;
            if (vehicleRequestDto.IsServiceBooked != null)
                isServiceBooked = factory.CreateService(vehicleRequestDto.IsServiceBooked.ServiceDate,
                                                           vehicleRequestDto.IsServiceBooked.ServiceHistory);

            IVehicle vehicle = factory.CreateVehicle(vehicleRequestDto.RegNumber, vehicleRequestDto.Model,
                                  vehicleRequestDto.Make, vehicleRequestDto.VehicleWeight,
                                  vehicleRequestDto.FirstDateInTraffic, vehicleRequestDto.IsRegistered,
                                  isServiceBooked, (IList<IVehicleService>)vehicleServices);
            return vehicle;
        }

        [HttpPost]
        [Route("api/registervehicle")]
        public IHttpActionResult CreateVehicle(VehicleRequestDto vehicleRequestDto)
        {
            var vehicle = CreateVehicleFromDto(vehicleRequestDto);

            vehicleRepository.CreateVehicle(vehicle);
            return Ok();
        }

        [HttpPost]
        [Route("api/getvehiclelist")]
        public IHttpActionResult GetVehicleList(SearchRequestDto searchRequestDto)
        {
            List<IVehicle> vehicleList = vehicleRepository.GetAllVehicles();
            vehicleList = vehicleService.Search(vehicleList, searchRequestDto.Search);
            return Ok(vehicleList);
        }

        [HttpGet]
        [Route("api/getvehicle")]
        public IHttpActionResult GetVehicle(string regNumber)
        {
            return Ok(vehicleRepository.GetVehicle(regNumber));
        }

        [HttpPost]
        [Route("api/removevehicle")]
        public IHttpActionResult DeleteVehicle(string regNumber)
        {
            vehicleRepository.DeleteVehicle(regNumber);
            return Ok();
        }

        [HttpPost]
        [Route("api/updatevehicle")]
        public IHttpActionResult UpdateVehicle(VehicleRequestDto vehicleRequestDto)
        {
            var vehicle = CreateVehicleFromDto(vehicleRequestDto);

            vehicleRepository.UpdateVehicle(vehicle);
            return Ok();
        }

        [HttpPost]
        [Route("api/bookservice")]
        public IHttpActionResult BookService(ServiceBookingDto serviceBookingDto)
        {
            {
            var factory = new VehicleFactory();
            IService service = factory.CreateService(serviceBookingDto.ServiceDate, serviceBookingDto.ServiceHistory);

            List<IVehicle> vehicles = new List<IVehicle>();
            foreach (string registrationNumber in serviceBookingDto.RegNumbers)
            {
                vehicles.Add(vehicleRepository.GetVehicle(registrationNumber));
            }

            vehicles = vehicleService.BookService(service, vehicles);

            foreach (IVehicle vehicle in vehicles)
            {
                vehicleRepository.UpdateVehicle(vehicle);
            }
            return Ok();
        }
        }

        [HttpPost]
        [Route("api/completeservice")]
        public IHttpActionResult CompleteService(RegNumberDto regNumberDto)
        {
            vehicleRepository.CompleteService(regNumberDto.RegNumber);
            return Ok();
        }
    }
}