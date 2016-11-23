using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        const string API_KEY = "AIzaSyBGAqm1Q8yXTrT4RxXjNZFf40iVXe7xdTo";

        [HttpGet("search")]
        // GET: api/cities/search?byName
        public async Task<string> Search([FromQuery]string byName)
        {
            return await loadCities(byName);
        }

        private async Task<string> loadCities(string byName)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(
                    "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&types=(cities)&key={1}",
                    byName,
                    API_KEY);
                var response = await client.GetStringAsync(address).ConfigureAwait(false);
                return response;
            }
        }

        [HttpGet("{placeId}")]
        public async Task<string> Get(string placeId)
        {
            return await loadDataAboutCity(placeId);
        }

        private async Task<string> loadDataAboutCity(string placeId)
        {
            using (var client = new HttpClient())
            {
                var address = string.Format(
                    "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}",
                    placeId,
                    API_KEY);
                var response = await client.GetStringAsync(address).ConfigureAwait(false);
                return response;
            }
        }
    }
}
