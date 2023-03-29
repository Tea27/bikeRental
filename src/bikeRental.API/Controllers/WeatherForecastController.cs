using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bikeRental.Application.Models;
using bikeRental.Application.Models.WeatherForecast;
using bikeRental.Application.Services;

namespace bikeRental.API.Controllers;

[Authorize]
public class WeatherForecastController : ApiController
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            ApiResult<IEnumerable<WeatherForecastResponseModel>>.Success(await _weatherForecastService.GetAsync()));
    }
}
