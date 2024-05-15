using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<Test, TestModel>()
            ;
    }
}