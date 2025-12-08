using ErrorOr;

namespace Products.Api.Errors;

public static class ApiErrors
{
    public static IResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }


        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    public static IResult Problem(Error error) =>
        Results.Problem(
            title: error.Description,
            statusCode: GetStatusCode(error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "code", error.Code }
            });

    private static IResult ValidationProblem(List<Error> errors)
    {
        return Results.ValidationProblem(
            errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
    }

    private static int GetStatusCode(ErrorType type) =>
        type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}