using System;
using System.Linq;
using WeatherApi.Business.Services.Forecast;
using WeatherApi.Business.Services.Forecast.Implementations;
using WeatherApi.Common.Configuration;

namespace WeatherApi.Business.Factories
{
    public class ForecastServiceFactory
    {
        private readonly ForecastAPISettings _apiSettings;

        public ForecastServiceFactory(ForecastAPISettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public IForecastService Create(string source)
        {
            switch (source)
            {
                case "FORECAST_IO":
                    return new ForecastIOService(_apiSettings.Providers.First(x=>x.Name == "forecast_io_api"));
                case "WORLD_WEATHER":
                    return new WorldWeatherService(_apiSettings.Providers.First(x => x.Name == "world_weather_api"));
                default:
                    throw new ArgumentException("Incorrect type of the source.");
            }
        }
    }
}
