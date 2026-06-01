using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult ProblemDetailsResponse(
        int statusCode,
        string title,
        string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        return StatusCode(statusCode, problemDetails);
    }

    protected ActionResult<T> ProblemDetailsResponse<T>(
        int statusCode,
        string title,
        string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        return StatusCode(statusCode, problemDetails);
    }

    protected ActionResult UnauthorizedProblem(
        string title = "Not authenticated",
        string detail = "The user is not authenticated.")
    {
        return ProblemDetailsResponse(
            StatusCodes.Status401Unauthorized,
            title,
            detail);
    }

    protected ActionResult<T> UnauthorizedProblem<T>(
        string title = "Not authenticated",
        string detail = "The user is not authenticated.")
    {
        return ProblemDetailsResponse<T>(
            StatusCodes.Status401Unauthorized,
            title,
            detail);
    }

    protected ActionResult BadRequestProblem(
        string title = "Bad request",
        string detail = "The request is invalid.")
    {
        return ProblemDetailsResponse(
            StatusCodes.Status400BadRequest,
            title,
            detail);
    }

    protected ActionResult<T> BadRequestProblem<T>(
        string title = "Bad request",
        string detail = "The request is invalid.")
    {
        return ProblemDetailsResponse<T>(
            StatusCodes.Status400BadRequest,
            title,
            detail);
    }

    protected ActionResult NotFoundProblem(
        string title = "Not found",
        string detail = "The requested resource was not found.")
    {
        return ProblemDetailsResponse(
            StatusCodes.Status404NotFound,
            title,
            detail);
    }

    protected ActionResult<T> NotFoundProblem<T>(
        string title = "Not found",
        string detail = "The requested resource was not found.")
    {
        return ProblemDetailsResponse<T>(
            StatusCodes.Status404NotFound,
            title,
            detail);
    }

    protected ActionResult ConflictProblem(
        string title = "Conflict",
        string detail = "The request could not be completed due to a conflict.")
    {
        return ProblemDetailsResponse(
            StatusCodes.Status409Conflict,
            title,
            detail);
    }

    protected ActionResult<T> ConflictProblem<T>(
        string title = "Conflict",
        string detail = "The request could not be completed due to a conflict.")
    {
        return ProblemDetailsResponse<T>(
            StatusCodes.Status409Conflict,
            title,
            detail);
    }

    protected ActionResult ServerErrorProblem(
        string title = "Server error",
        string detail = "An error occurred while processing the request.")
    {
        return ProblemDetailsResponse(
            StatusCodes.Status500InternalServerError,
            title,
            detail);
    }

    protected ActionResult<T> ServerErrorProblem<T>(
        string title = "Server error",
        string detail = "An error occurred while processing the request.")
    {
        return ProblemDetailsResponse<T>(
            StatusCodes.Status500InternalServerError,
            title,
            detail);
    }
}