using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using VehicleRegistry.DTO.dto;
using VehicleRegistry.Client.Models;
using VehicleRegistry.Client.Models.Account;
using System.Net;

namespace VehicleRegistry.Client.Controllers
{
    public class AccountController : Controller
    {

        private readonly EndPoints endPoints;

        public AccountController()
        {
            endPoints = new EndPoints();
        }

        public ActionResult LoginView()
        {
            return View("Login");
        }

        public ActionResult CreateAccountView()
        {
            return View("CreateAccount");
        }

        private TokenRecieverModel GetToken(LoginModel loginModel)
        {
            TokenRecieverModel AccessToken = null;
            var httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };

            var httpClient = new HttpClient(httpClientHandler);

            var stringContent = new StringContent(loginModel.GetTokenRequestString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage tokenResponse = httpClient.PostAsync(endPoints.GetToken, stringContent).Result;
            {
                if (tokenResponse != null)
                {
                    var jString = tokenResponse.Content.ReadAsStringAsync().Result;
                    dynamic response = JsonConvert.DeserializeObject<dynamic>(jString);
                    AccessToken = response.access_token;
                }
                return AccessToken;
            }
        }

        public ActionResult Login(LoginModel loginModel)
        {
            TokenRecieverModel token = GetToken(loginModel);
            Session["TokenKey"] = token.Token_type + " " + token.Access_token;

            return RedirectToAction("Home", "VehicleRegistry");
        }

        public ActionResult CreateAccount(AccountModel AccountModel)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Session["TokenKey"].ToString());
                AccountRequestDto request = AccountModel.GetDto();
                var jsonRequestString = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(jsonRequestString, UnicodeEncoding.UTF8, "application/json");
                var response = client.PostAsync(endPoints.CreateAccount, stringContent).Result;
            }
            ViewBag.message = "Account created";
            return View("Login");
        }
    }
}