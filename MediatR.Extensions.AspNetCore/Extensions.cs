using FluentResults;
using MediatR.Extensions.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MediatR.Extensions.AspNetCore;

public static class Extensions
{
    public static IResult ToHttpResult<TValue>(this IResult<TValue> result) where TValue : notnull
    {
        if (result.IsFailed && result.Errors.Count != 0 && result.Errors.Exists(x => x is IValidationError))
            return ToValidationFailure(result.Errors.OfType<IValidationError>().First());

        if (result.IsFailed)
            return Results.BadRequest(result.Errors);

        return Results.Ok(result.Value);
    }

    internal static IResult ToValidationFailure(IValidationError validationError)
    {
        var errors = validationError.Errors.GroupBy(x => x.PropertyName)
                                           .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());

        return Results.ValidationProblem(errors);
    }
}
