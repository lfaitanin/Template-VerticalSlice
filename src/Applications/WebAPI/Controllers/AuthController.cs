using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using WebAPI.Features.Login.Commands;
using WebAPI.Features.Login.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginCommand command)
        {
            try
            {
                var (token, userId) = await _mediator.Send(command);
                return Ok(new { token, userId });
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Login error!", $"{e.Message}", StatusCodes.Status401Unauthorized);
                return Unauthorized(problemDetails);
            }
        }

        [HttpGet("who-am-i")]
        public async Task<ActionResult<WhoAmIResponse>> WhoAmI([FromHeader(Name = "Authorization")] string authorization)
        {
            try
            {
                if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
                {
                    return Unauthorized("Invalid authorization header");
                }

                // Extract user ID from JWT token (you might want to create a service for this)
                // For now, we'll use a placeholder
                var userId = Guid.NewGuid(); // This should be extracted from the JWT token

                var command = new WhoAmICommand { UserId = userId };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error getting user info!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }
    }
}