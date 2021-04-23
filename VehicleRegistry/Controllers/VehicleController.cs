using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using VehicleRegistry.Client.Models;
using VehicleRegistry.DTO.dto;
using VehicleRegistry.Models.Vehicle;
using VehicleRegistry.Client.Models.VehicleRegistry;

namespace VehicleRegistry.Client.Controllers
{
    public class VehicleController : Controller
    {
        private readonly EndPoints endPoints;

        public VehicleController()
        {
            endPoints = new EndPoints();
        }
        public ActionResult HomeView()
        {
            return View("Home");
        }
        public ActionResult BookServiceView()
        {
            return View("BookService");
        }
        public ActionResult CreateVehicleView()
        {
            return View("CreateVehicle");
        }

        public ActionResult CreateVehicle(VehicleModel vehiclemodel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                VehicleRequestDto request = vehiclemodel.GetDto();
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.CreateVehicle, stringContent).Result;
            }
            ViewBag.message = "Vehicle has been registered!";
            return View("Home");
        }

        public ActionResult GetVehicleList(string search)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                SearchRequestDto request = new SearchRequestDto
                {
                    Search = search
                };
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.GetVehicleList, stringContent).Result;
                if (response != null)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(jsonString);
                    string listAsText = "";
                    foreach (VehicleModel vehicle in vehicles)
                    {
                        string vehicleAsText = $"{vehicle.RegNumber} | {vehicle.Model} | {vehicle.Make}";
                        listAsText += $"{vehicleAsText}\n";
                    }
                    ViewBag.Content = listAsText;
                }
            }
            return View("GetVehicleList");
        }

        public ActionResult GetVehicle(string regNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                VehicleRequestDto request = new VehicleRequestDto
                {
                    RegNumber = regNumber
                };
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.GetVehicle, stringContent).Result;
                if (response != null)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var vehicle = JsonConvert.DeserializeObject<VehicleModel>(jsonString);

                    ViewBag.RegistrationNumber = vehicle.RegNumber;
                    ViewBag.Model = vehicle.Model;
                    ViewBag.Make = vehicle.Make;
                    ViewBag.VehicleType = vehicle.GetVehicleType();
                    ViewBag.VehicleWeight = vehicle.VehicleWeight;
                    ViewBag.FirstDateInTrafic = vehicle.FirstDateInTraffic.ToShortDateString();
                    ViewBag.IsRegistered = vehicle.IsRegistered.ToString();

                    if (vehicle.IsServiceBooked != null)
                    {
                        ViewBag.Date = vehicle.IsServiceBooked.ServiceDate.ToShortDateString();
                        ViewBag.Description = vehicle.IsServiceBooked.ServiceHistory;
                    }

                    if (vehicle.ServiceHistory.Count > 0)
                    {
                        string listAsText = "";
                        System.Collections.IList list = vehicle.ServiceHistory;
                        for (int i = 0; i < list.Count; i++)
                        {
                            ServiceModel service = (ServiceModel)list[i];
                            listAsText += $"{service.ServiceDate.ToShortDateString()} \n";
                            listAsText += $"{service.ServiceHistory} \n \n";
                        }
                        ViewBag.ServiceHistory = listAsText;
                    }
                }
            }
            return View("GetVehicle");
        }

        public ActionResult RemoveVehicle(string regNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                VehicleRequestDto request = new VehicleRequestDto
                {
                    RegNumber = regNumber
                };
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                _ = client.PostAsync(endPoints.RemoveVehicle, stringContent).Result;
            }
            ViewBag.Message = "Vehicle has been removed!";
            return View("Home");
        }
        public ActionResult UpdateVehicleView(string regNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                VehicleRequestDto request = new VehicleRequestDto
                {
                    RegNumber = regNumber
                };
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.GetVehicle, stringContent).Result;
                if (response != null)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var vehicle = JsonConvert.DeserializeObject<VehicleModel>(jsonString);

                    ViewBag.RegistrationNumber = vehicle.RegNumber;
                    ViewBag.Model = vehicle.Model;
                    ViewBag.Make = vehicle.Make;
                    ViewBag.VehicleType = vehicle.GetVehicleType();
                    ViewBag.VehicleWeight = vehicle.VehicleWeight;
                    var date = vehicle.FirstDateInTraffic.Date;
                    string year = date.Year.ToString();
                    string month = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
                    string day = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
                    ViewBag.FirstDateInTrafic = $"{year}-{month}-{day}";
                    ViewBag.IsRegistered = vehicle.IsRegistered;

                    if (vehicle.IsServiceBooked != null)
                    {
                        date = vehicle.IsServiceBooked.ServiceDate.Date;
                        year = date.Year.ToString();
                        month = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
                        day = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
                        ViewBag.Date = $"{year}-{month}-{day}";
                        ViewBag.Description = vehicle.IsServiceBooked.ServiceHistory;
                    }
                }
            }
            return View("UpdateVehicle");
        }

        public ActionResult UpdateVehicle(VehicleModel vehicleModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                VehicleRequestDto request = vehicleModel.GetDto();
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.UpdateVehicle, stringContent).Result;
            }
            ViewBag.Message = "Vehicle has been updated!";
            return View("Home");
        }

        public ActionResult BookService(ServiceBookingModel serviceBookingModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                ServiceBookingDto request = serviceBookingModel.GetDto();
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.BookService, stringContent).Result;
            }
            ViewBag.Message = "Service has been booked!";
            return View("Home");
        }

        public ActionResult CompleteService(string regNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                RegNumberDto request = new RegNumberDto
                {
                    RegNumber = regNumber
                };

                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, Encoding.UTF8, "application/json");
                _ = client.PostAsync(endPoints.CompleteService, stringContent).Result;
            }
            ViewBag.Message = "Service has been completed!";
            return View("Home");
        }
    }
}