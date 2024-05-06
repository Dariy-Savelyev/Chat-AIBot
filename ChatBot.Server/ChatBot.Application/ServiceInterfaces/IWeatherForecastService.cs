using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IWeatherForecastService : IBaseService
{
    IEnumerable<WeatherForecast> GetForecast();
}