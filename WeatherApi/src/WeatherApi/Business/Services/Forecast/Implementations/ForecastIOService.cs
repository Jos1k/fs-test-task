using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApi.Common.Configuration;
using WeatherApi.Models;

namespace WeatherApi.Business.Services.Forecast.Implementations
{
    public class ForecastIOService : IForecastService
    {
        private readonly ForecastProviderSetting _providerSetting;

        public ForecastIOService(ForecastProviderSetting providerSetting)
        {
            _providerSetting = providerSetting;
        }

        public async Task<WeeklyForecast> LoadAsync(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(_providerSetting.Url, _providerSetting.LicenseKey, longitude, latitude);
                var response = await client.GetStringAsync(address).ConfigureAwait(false);

                return ParseResponse(response);
            }
        }
        private WeeklyForecast ParseResponse(string response)
        {
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            var currentlyWeather = jsonResponse["currently"];
            var todayForecast = new TodayForecast
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Humidity = currentlyWeather["humidity"],
                Pressure = currentlyWeather["pressure"],
                CloudCover = currentlyWeather["cloudCover"],
                Temperature = currentlyWeather["temperature"],
                ApparentTemperature = currentlyWeather["apparentTemperature"]
            };

            var currentDate = DateTime.Now.Date;
            var futureForecasts = jsonResponse["daily"]["data"];

            var futureDayForecasts = new List<FutureDayForecast>();
            foreach (var futureForecast in futureForecasts)
            {
                long seconds = futureForecast["time"];
                DateTime targetDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                targetDate = targetDate.AddSeconds(seconds);

                if (targetDate.Date > currentDate)
                {
                    var forecastForFutureDay = new FutureDayForecast()
                    {
                        Date = targetDate.ToString("yyyy-MM-dd"),
                        Humidity = futureForecast["humidity"],
                        Pressure = futureForecast["pressure"],
                        CloudCover = futureForecast["cloudCover"],
                        TemperatureMin = futureForecast["temperatureMin"],
                        TemperatureMax = futureForecast["temperatureMax"],
                        ApparentTemperatureMin = futureForecast["apparentTemperatureMin"],
                        ApparentTemperatureMax = futureForecast["apparentTemperatureMax"]
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