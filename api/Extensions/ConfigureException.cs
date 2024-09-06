using Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Extensions;

public static class ConfigureException
{
    public static void ConfigureExceptionHandler(this WebApplication application)
    {
        application.UseExceptionHandler(e =>
            e.Run(async context =>
            {
                var handler = context.Features.Get<IExceptionHandlerFeature>();
                if (handler != null)
                {
                    var problemDetailsFactory =
                        application.Services.GetRequiredService<ProblemDetailsFactory>();

                    ProblemDetails problemDetails;
                    int statusCode;

                    (problemDetails, statusCode) = handleException(
                        context,
                        handler,
                        problemDetailsFactory
                    );

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            })
        );
    }

    private static (ProblemDetails problemDetails, int statusCode) handleException(
        HttpContext context,
        IExceptionHandlerFeature handler,
        ProblemDetailsFactory problemDetailsFactory
    ) =>
        handler.Error switch
        {
            ApiException e => (
                problemDetailsFactory.CreateProblemDetails(
                    context,
                    e.StatusCode,
                    e.Title,
                    e.Detail
                ),
                e.StatusCode
            ),
            _ => (
                problemDetailsFactory.CreateProblemDetails(
                    context,
                    StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error",
                    detail: "An unexpected error occurred."
                ),
                StatusCodes.Status500InternalServerError
            ),
        };
}
