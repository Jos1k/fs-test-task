using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApi.Common.Configuration;
using WeatherApi.Models;

namespace WeatherApi.Business.Services.Forecast.Implementations
{
    public class WorldWeatherService : IForecastService
    {
        private readonly ForecastProviderSetting _providerSetting;

        public WorldWeatherService(ForecastProviderSetting providerSetting)
        {
            _providerSetting = providerSetting;
        }

        public async Task<WeeklyForecast> LoadAsync(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(_providerSetting.Url, _providerSetting.LicenseKey, longitude, latitude );
                var response = await client.GetStringAsync(address).ConfigureAwait(false);
                return ParseResponse(response);
            }
        }

        public WeeklyForecast ParseResponse(string response)
        {
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            var currentlyWeather = jsonResponse["data"]["current_condition"][0];
            var todayForecast = new TodayForecast
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Humidity = float.Parse((string)currentlyWeather["humidity"]) / 1000,
                Pressure = float.Parse((string)currentlyWeather["pressure"]),
                CloudCover = float.Parse((string)currentlyWeather["cloudcover"]) / 100,
                Temperature = float.Parse((string)currentlyWeather["temp_F"]),
                ApparentTemperature = float.Parse((string)currentlyWeather["FeelsLikeF"])
            };

            var currentDate = DateTime.Now.Date;
            var futureForecasts = jsonResponse["data"]["weather"];

            var futureDayForecasts = new List<FutureDayForecast>();
            foreach (var futureForecast in futureForecasts)
            {
                var forecastDate = DateTime.Parse((string)futureForecast["date"]);
                if (forecastDate > currentDate)
                {
                    var weatherOfFutureDay = futureForecast["hourly"][0];

                    var forecastForFutureDay = new FutureDayForecast()
                    {
                        Date = futureForecast["date"],
                        Humidity = float.Parse((string)weatherOfFutureDay["humidity"]) / 100,
                        Pressure = float.Parse((string)weatherOfFutureDay["pressure"]),
                        CloudCover = float.Parse((string)weatherOfFutureDay["cloudcover"]) / 100,
                        TemperatureMin = futureForecast["mintempF"],
                        TemperatureMax = futureForecast["maxtempF"],
                        ApparentTemperatureMin = float.Parse((string)weatherOfFutureDay["FeelsLikeF"]),
                        ApparentTemperatureMax = float.Parse((string)weatherOfFutureDay["FeelsLikeF"])
                    };
                    futureDayForecasts.Add(forecastForFutureDay);
                }
            }

            return new WeeklyForecast
            {
                Current = todayForecast,
                FutureDays = futureDayForecasts
            };
        }
    }
}