using System.Net.Http;
using System.Threading.Tasks;
using WeatherApi.Common.Configuration;

namespace WeatherApi.Business.Services.Citites
{
    public class CitiesService
    {
        private readonly CityAPISettings _cityApiSettings;

        public CitiesService(CityAPISettings cityApiSettings)
        {
            _cityApiSettings = cityApiSettings;
        }

        public async Task<string> LoadCities(string byName)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(_cityApiSettings.CityByNameUrl, byName, _cityApiSettings.LicenseKey);
                var response = await client.GetStringAsync(address).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<string> LoadDataAboutCity(string placeId)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(_cityApiSettings.CityByIdUrl, placeId, _cityApiSettings.LicenseKey);
                var response = await client.GetStringAsync(address).ConfigureAwait(false);
                return response;
            }
        }
    }
}
