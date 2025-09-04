using API.Domain.Common;

namespace API.API.Common;

public static class HttpResultsMapper
{
    public static IResult ToHttp<T>(
        Result<T> result,
        Func<T, IResult> onSuccess,
        Func<string, bool>? notFoundWhen = null,
        Func<string, bool>? conflictWhen = null)
    {
        if (result.IsSuccess) return onSuccess(result.Value!);

        var error = result.Error ?? "Unknown error";

        if (notFoundWhen is not null && notFoundWhen(error))
            return Results.Problem(title: "Not Found", detail: error, statusCode: StatusCodes.Status404NotFound);

        if (conflictWhen is not null && conflictWhen(error))
            return Results.Problem(title: "Conflict", detail: error, statusCode: StatusCodes.Status409Conflict);

        return Results.Problem(title: "Bad Request", detail: error, statusCode: StatusCodes.Status400BadRequest);
    }

    public static IResult ToHttp(
        Result result,
        Func<IResult> onSuccess,
        Func<string, bool>? notFoundWhen = null,
        Func<string, bool>? conflictWhen = null)
    {
        if (result.IsSuccess) return onSuccess();

        var error = result.Error ?? "Unknown error";

        if (notFoundWhen is not null && notFoundWhen(error))
            return Results.Problem(title: "Not Found", detail: error, statusCode: StatusCodes.Status404NotFound);

        if (conflictWhen is not null && conflictWhen(error))
            return Results.Problem(title: "Conflict", detail: error, statusCode: StatusCodes.Status409Conflict);

        return Results.Problem(title: "Bad Request", detail: error, statusCode: StatusCodes.Status400BadRequest);
    }
}
