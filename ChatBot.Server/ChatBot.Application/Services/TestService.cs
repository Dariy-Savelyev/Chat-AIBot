﻿using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Exceptions;
using ChatBot.CrossCutting.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class TestService(ITestRepository testRepository, IMapper mapper) : ITestService
{
    public async Task<IEnumerable<TestModel>> GetAllAsync(TestModel model)
    {
        var tests = await testRepository.GetAllAsync();
        var result = mapper.Map<IEnumerable<TestModel>>(tests);
        return result;
    }
}