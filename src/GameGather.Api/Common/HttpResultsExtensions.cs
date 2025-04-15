using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace GameGather.Api.Common;

public static class HttpResultsExtensions
{
    public static IResult Ok(object value)
    {
        return Results.Ok(value);
    }
    public static IResult Problem(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = firstError.Description
        };

        if (errors is not null)
        {
            problemDetails.Extensions.Add("errorCodes", errors.Select(x => x.Code));
        }

        return Results.Problem(
            detail: problemDetails.Title,
            statusCode: problemDetails.Status,
            extensions: problemDetails.Extensions);
    }
}