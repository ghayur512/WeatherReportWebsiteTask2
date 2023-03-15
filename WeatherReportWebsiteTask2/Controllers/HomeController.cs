using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherReportWebsiteTask2.BL;
using WeatherReportWebsiteTask2.Models;

namespace WeatherReportWebsiteTask2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WeatherHelper helper = new WeatherHelper();
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult GetWeatherData()
        {
            var result =  getWeatherDetailsAsync();
            return Json(new { data = true });
        }
        public async Task<bool> getWeatherDetailsAsync()
        {
            string connectionString = Configuration.GetConnectionString("dbConnection");
            try
            {
                string[] cityArr = { "New York", "Virginia", "Islamabad", "Lahore", "Mumbai", "New Delhi", "London", "Paris", "Istanbul", "Beijing" };

                HttpClient client = new HttpClient();
                
                DataResponse response1 = new DataResponse();

                foreach (var item in cityArr)
                {
                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get
                    };
                    httpRequestMessage.RequestUri = new Uri("http://api.weatherstack.com/current?access_key=b0dc114274a372edbdedf00e859f9fd8&query=" + item);
                    var response = await client.SendAsync(httpRequestMessage);


                    if (response.IsSuccessStatusCode)
                    {
                        response1 = await response.Content.ReadAsAsync<DataResponse>();
                        WeatherDataResponseModel weatherData = new WeatherDataResponseModel()
                        {
                            name = response1.location.name,
                            country = response1.location.country,
                            temperature = response1.current.temperature,
                            wind_speed = response1.current.wind_speed,
                            cloudcover = response1.current.cloudcover
                        };

                        var addinDb = helper.saveWeatherData(weatherData, connectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public ActionResult GetTempratureTrendData()
        {
            string connectionString = Configuration.GetConnectionString("dbConnection");
            var result = helper.getTemparatureData(connectionString);
            return Json(new { data = result });
        }

        public ActionResult GetWindSpeedTrendData()
        {
            string connectionString = Configuration.GetConnectionString("dbConnection");
            var result = helper.getWindSpeedData(connectionString);
            return Json(new { data = result });
        }

        public ActionResult GetWindTemperatureTrendData()
        {
            string connectionString = Configuration.GetConnectionString("dbConnection");
            var result = helper.getTemperatureWindTrend(connectionString);
            return Json(new { data = result });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
