using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using WebAPI.Features.Profile.Commands;
using WebAPI.Features.Profile.Queries;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-profile")]
        public async Task<ActionResult<int>> CreateProfile(CreateProfileCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error creating profile!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("get-profiles")]
        public async Task<ActionResult<List<WebAPI.Features.Profile.Models.Profile>>> GetProfiles(string documentNumber)
        {
            try
            {
                var query = new GetProfilesQuery { documentNumber = documentNumber };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error getting profiles!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }
    }
} 