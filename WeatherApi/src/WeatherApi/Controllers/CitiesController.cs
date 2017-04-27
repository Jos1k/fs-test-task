using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherApi.Business.Models.Configuration;
using WeatherApi.Business.Services.Citites;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly CitiesService _citiesService;
        public CitiesController(IOptions<CityAPISettings> cityApiSettings)
        {
            _citiesService = new CitiesService(cityApiSettings.Value);
        }

        [HttpGet("search")]
        // GET: api/cities/search?byName
        public async Task<string> Search([FromQuery]string byName)
        {
            return await _citiesService.LoadCities(byName);
        }

        [HttpGet("{placeId}")]
        public async Task<string> Get(string placeId)
        {
            return await _citiesService.LoadDataAboutCity(placeId);
        }


    }
}
