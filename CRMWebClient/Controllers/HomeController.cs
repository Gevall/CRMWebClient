using CRMWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace CRMWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            this._logger = Program.logFactory.CreateLogger<HomeController>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отображение списка проектов отдела
        /// </summary>
        /// <param name="asingEmploye"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ProjectView(AsignTripToEmploye asingEmploye)
        {

            _logger.LogInformation("debug: ");
            try
            {
                var trips = GetTrips().Result;
                ViewBag.Trips = trips;
                var employes = GetEmployes().Result;
                ViewBag.Employes = employes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                ViewBag.Title = null;
            }
            return View();
        }

        public IActionResult TestWindow()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AsignTrip(string employeId, string tripStart, string tripId)
        {
            AsignTripToEmploye asignTripToEmploye = new AsignTripToEmploye()
            {
                tripId = Int32.Parse(tripId),
                employeId = Int32.Parse(employeId),
                dateTrip = DateTime.Parse(tripStart)
            };
            _logger.LogInformation("debug: id:{0} | tripId: {1} | tripdate: {2}", asignTripToEmploye.employeId, asignTripToEmploye.tripId, asignTripToEmploye.dateTrip);
            var result = await AsignTripToEmploye(asignTripToEmploye);
            if (result == HttpStatusCode.OK)
            {
                _logger.LogInformation("Resilt: OK");
            }
            else
            {
                _logger.LogInformation("Result: ERROR");
            }
            return Redirect("~/Home/ProjectView");
        }

        /// <summary>
        /// Получение списка командировок из базы
        /// </summary>
        /// <returns>Список командировок в list</returns>
        private async Task<List<Trips>> GetTrips()
        {
            List<Trips> trips = new List<Trips>();
            var client = _httpClientFactory.CreateClient("GetTrips");
            var response = await client.GetAsync("http://localhost:5000/gettrips");
            if (response.IsSuccessStatusCode)
            {
                var textRespose = await response.Content.ReadAsStringAsync();
                trips = JsonSerializer.Deserialize<List<Trips>>(textRespose);
            }
            return trips;
        }

        /// <summary>
        /// Получение списка сотрудников из базы
        /// </summary>
        /// <returns>Список сотрудников в List</returns>
        private async Task<List<Employes>> GetEmployes()
        {
            List<Employes> employes = new List<Employes>();
            var client = _httpClientFactory.CreateClient("GetEmployes");
            var response = await client.GetAsync("http://localhost:5000/getemployes");
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                employes = JsonSerializer.Deserialize<List<Employes>>(jsonResult);
            }
            return employes;
        }

        private async Task<HttpStatusCode> AsignTripToEmploye(AsignTripToEmploye asignTrip)
        {
            var asignClient = _httpClientFactory.CreateClient("AsignTrip");
            
            var responseJson = asignClient.PostAsync(
                requestUri: @"http://localhost:5000/asigntrip",
                content: new StringContent(JsonSerializer.Serialize(asignTrip),
                Encoding.UTF8,
                mediaType: "application/json"))
                .Result;
            return HttpStatusCode.OK;
        }
    }
}
