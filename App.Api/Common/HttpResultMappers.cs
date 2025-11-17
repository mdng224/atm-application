using App.Application.Common.Results;

namespace App.Api.Common;

internal static class HttpResultMappers
{
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onOk)
        => result.IsSuccess ? onOk(result.Value!) : MapError(result.Error);

    private static IResult MapError(Error? error)
    {
        if (error is null)
        {
            return Results.Problem(
                detail: "An unknown error occurred.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var e = error.Value;

        // Convention: "<category>.<field>" e.g. "validation.amount"
        var parts = e.Code.Split('.', 2, StringSplitOptions.TrimEntries);
        var category = parts[0];
        var field = parts.Length > 1 ? parts[1] : null;

        return category switch
        {
            // Validation errors (bad input)
            "validation" => Results.ValidationProblem(
                errors: new Dictionary<string, string[]>
                {
                    [field ?? ""] = [e.Message]
                },
                title: "Validation Error",
                statusCode: StatusCodes.Status400BadRequest,
                extensions: new Dictionary<string, object?> { ["code"] = e.Code }),

            // Conflict (e.g., insufficient funds, or business rule failures)
            "conflict" => Results.Problem(
                title: "Conflict",
                detail: e.Message,
                statusCode: StatusCodes.Status409Conflict,
                extensions: BuildExtensions(e.Code, field, e.Message)),

            // Not found
            "not_found" => Results.Problem(
                title: "Not Found",
                detail: e.Message,
                statusCode: StatusCodes.Status404NotFound,
                extensions: new Dictionary<string, object?> { ["code"] = e.Code }),

            // Default → 400 Bad Request
            _ => Results.Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status400BadRequest,
                extensions: new Dictionary<string, object?> { ["code"] = e.Code })
        };
    }

    private static Dictionary<string, object?> BuildExtensions(string code, string? field, string message)
    {
        var ext = new Dictionary<string, object?> { ["code"] = code };

        if (!string.IsNullOrWhiteSpace(field))
        {
            ext["errors"] = new Dictionary<string, string[]>
            {
                [field] = [message]
            };
        }

        return ext;
    }
}
