using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Business.Services.Forecast
{
    public interface IForecastService
    {
        Task<WeeklyForecast> LoadAsync(double latitude, double longitude);
    }
}
