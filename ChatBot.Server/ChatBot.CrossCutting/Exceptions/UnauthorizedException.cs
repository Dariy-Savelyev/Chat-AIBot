﻿using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

public sealed class UnauthorizedException : BaseException
{
    public UnauthorizedException(IReadOnlyCollection<ResponseError> errors)
       : base(errors, "Unauthorized. One or more validation errors occurred")
       => Errors = errors;
}