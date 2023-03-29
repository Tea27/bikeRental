using bikeRental.Application.Models.WeatherForecast;

namespace bikeRental.Application.Services;

public interface IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecastResponseModel>> GetAsync();
}
