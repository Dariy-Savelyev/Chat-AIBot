using AutoMapper;
using ChatBot.Application.Models;

namespace ChatBot.Application.MapperProfiles;

public class WeatherForecastProfile: Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<WeatherForecast, WeatherForecast>()
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(s => s.Date))
            .ForMember(
                dest => dest.Summary,
                opt => opt.MapFrom(s => s.Summary))
            ;
    }
}