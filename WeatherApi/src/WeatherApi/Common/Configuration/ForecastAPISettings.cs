using System.Collections.Generic;

namespace WeatherApi.Common.Configuration
{
    public class ForecastAPISettings
    {
        public List<ForecastProviderSetting> Providers { get; set; }
    }
}
