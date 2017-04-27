using System.Collections.Generic;

namespace WeatherApi.Business.Models.Configuration
{
    public class ForecastApiSettings
    {
        public List<ForecastProviderSetting> Providers { get; set; }
    }
}
