using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherApi.Business.Factories;
using WeatherApi.Common.Configuration;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    public class ForecastController : Controller
    {
        private readonly ForecastServiceFactory _forecastServiceFactory;

        public ForecastController(IOptions<ForecastAPISettings> forecastApiSettings)
        {
            _forecastServiceFactory = new ForecastServiceFactory(forecastApiSettings.Value);
        }

        // GET: api/forecast?longitude&latitude&source
        public async Task<WeeklyForecast> Get([FromQuery]double latitude, [FromQuery]double longitude, [FromQuery]string source)
        {
            return await _forecastServiceFactory.Create(source).LoadAsync(longitude, latitude);
        }
    }
}
