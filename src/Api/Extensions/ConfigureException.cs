using Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

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

                    var isHandled = handleException(
                        context,
                        handler,
                        problemDetailsFactory,
                        out ProblemDetails problemDetails,
                        out int statusCode
                    );

                    if (!isHandled)
                        throw handler.Error;

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            })
        );
    }

    private static bool handleException(
        HttpContext context,
        IExceptionHandlerFeature handler,
        ProblemDetailsFactory problemDetailsFactory,
        out ProblemDetails problemDetails,
        out int statusCode
    )
    {
        switch (handler.Error)
        {
            case ApiException e:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    e.StatusCode,
                    e.Title,
                    e.Detail
                );
                statusCode = e.StatusCode;
                return true;

            default:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error",
                    detail: "An unexpected error occurred."
                );
                statusCode = StatusCodes.Status500InternalServerError;
                return false;
        }
    }
}


/* private static (ProblemDetails problemDetails, int statusCode) handleException(
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
        }; */
