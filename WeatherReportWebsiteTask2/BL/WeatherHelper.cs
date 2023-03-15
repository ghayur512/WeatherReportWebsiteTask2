using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WeatherReportWebsiteTask2.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WeatherReportWebsiteTask2.BL
{
    public class WeatherHelper
    {
        public bool saveWeatherData (WeatherDataResponseModel request, string connectionString)
        {
            try
            {
             
                string sql = "INSERT INTO [dbo].[WeatherReport]([City],[Country],[Temprature],[Clouds],[WindSpeed],[CreatedDate]) VALUES (@City, @Country, @Temprature, @Clouds, @WindSpeed, @CreatedDate)";

                object[] parameters = { new { City = request.name, Country = request.country, Temprature = request.temperature, Clouds = request.cloudcover, WindSpeed = request.wind_speed, CreatedDate = request.CreatedDate } };

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Execute(sql, parameters);
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public List<WeatherDataResponseModel> getTemparatureData(string connectionString)
        {
            List<WeatherDataResponseModel> responseList = new List<WeatherDataResponseModel>();
            try
            {                
                string sql = "Select City as name, Min(Temprature) as temperature from [WeatherReport] group by City order by 2";
               
                using (var conn = new SqlConnection(connectionString))
                {
                    responseList = conn.Query<WeatherDataResponseModel>(
                        sql,
                        "",
                        commandType: CommandType.Text
                    )
                    .ToList();                   
                }

                return responseList;
            }
            catch (Exception ex)
            {
                return responseList;
            }

        }

        public List<WeatherDataResponseModel> getWindSpeedData(string connectionString)
        {
            List<WeatherDataResponseModel> responseList = new List<WeatherDataResponseModel>();
            try
            {
                string sql = "Select City as name, Max(WindSpeed) as wind_speed from [WeatherReport] group by City order by 2";

                using (var conn = new SqlConnection(connectionString))
                {
                    responseList = conn.Query<WeatherDataResponseModel>(
                                    sql,
                                    "",
                                    commandType: CommandType.Text).ToList();
                }

                return responseList;
            }
            catch (Exception ex)
            {
                return responseList;
            }

        }

        public List<WeatherDataResponseModel> getTemperatureWindTrend(string connectionString)
        {
            List<WeatherDataResponseModel> responseList = new List<WeatherDataResponseModel>();
            try
            {
                string sql = "EXEC [GetTempWindTrend]";

                using (var conn = new SqlConnection(connectionString))
                {
                    responseList = conn.Query<WeatherDataResponseModel>(
                                    sql,
                                    "",
                                    commandType: CommandType.Text).ToList();
                }

                return responseList;
            }
            catch (Exception ex)
            {
                return responseList;
            }

        }
    }
}
