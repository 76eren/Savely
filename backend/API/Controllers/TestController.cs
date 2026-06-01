using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public sealed class TestController : BaseApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello, World!");
    }

    [HttpGet("error")]
    public IActionResult GetThrowError()
    {
        return BadRequestProblem(
            title: "This is a bad request",
            detail: "The request was invalid. Please check your input and try again.");
    }
    
}