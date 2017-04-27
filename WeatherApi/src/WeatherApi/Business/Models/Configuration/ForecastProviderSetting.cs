namespace WeatherApi.Business.Models.Configuration
{
    public class ForecastProviderSetting
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string LicenseKey { get; set; }
    }
}