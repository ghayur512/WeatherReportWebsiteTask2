using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReportWebsiteTask2.Models
{
    public class DataResponse
    {
        public Location location { get; set; }
        public Current current { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Current
    {
        public string temperature { get; set; }
        public string wind_speed { get; set; }
        public string cloudcover { get; set; }       
    }

    public class WeatherDataResponseModel
    {
        public string name { get; set; }
        public string country { get; set; }
        public string temperature { get; set; }
        public string wind_speed { get; set; }
        public string cloudcover { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
