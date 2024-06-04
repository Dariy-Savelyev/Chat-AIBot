using ChatBot.CrossCutting.Exceptions;
using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Extensions;

public class ExceptionHelper
{
    public static void ThrowArgumentExceptionCryptedString()
    {
        ThrowArgumentException("CryptedString", "Invalid Key");
    }

    public static void ThrowArgumentException(string fieldName, string messages)
    {
        throw new ArgumentValidationException(
        [
            new(fieldName, [messages])
        ]);
    }

    public static ArgumentValidationException GetArgumentException(string fieldName, string messages)
    {
        throw new ArgumentValidationException(
        [
            new(fieldName, [messages])
        ]);
    }

    public static void ThrowConflictException(string fieldName, string messages)
    {
        throw new ConflictException(
        [
            new(fieldName, [messages])
        ]);
    }

    public static ConflictException GetConflictException(string messages)
    {
        return new ConflictException(
        [
            new(string.Empty, [messages])
        ]);
    }

    public static void ThrowNotFoundException(string fieldName, string messages)
    {
        throw new NotFoundException(
        [
            new(fieldName, [messages])
        ]);
    }

    public static NotFoundException GetNotFoundException(string messages)
    {
        return new NotFoundException(
        [
            new(string.Empty, [messages])
        ]);
    }

    public static void ThrowGoneException(string messages)
    {
        throw new GoneException(
        [
            new(string.Empty, [messages])
        ]);
    }

    public static void ThrowForbiddenException(string messages)
    {
        throw GetForbiddenException(messages);
    }

    public static ForbiddenException GetForbiddenException(string messages)
    {
        throw new ForbiddenException(
        [
            new(string.Empty, [messages])
        ]);
    }

    public static void ThrowUnauthorizedException(string messages)
    {
        throw GetUnauthorizedException(messages);
    }

    public static UnauthorizedException GetUnauthorizedException(string messages)
    {
        return new UnauthorizedException(
        [
             new(string.Empty, [messages])
        ]);
    }

    public static void ThrowFailedDependencyException(string messages)
    {
        throw new FailedDependencyException(
        [
             new(string.Empty, [messages])
        ]);
    }
}