using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using WebAPI.Features.User.Queries;
using WebAPI.Features.User.Commands;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<int>> CreateUser(CreateUserCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error in registration!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("validate-registration")]
        public async Task<ActionResult<Guid>> ValidateFirstAccess(string documentNumber)
        {
            try
            {
                var query = new ValidateUserQuery { DocumentNumber = documentNumber };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error validating registration!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<List<WebAPI.Features.User.Models.User>>> GetUsers()
        {
            try
            {
                var query = new GetUsersQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error getting users!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpPost("send-confirmation-code")]
        public async Task<ActionResult<bool>> SendConfirmationCode(SendConfirmationCodeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error sending confirmation code!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpPost("validate-confirmation-code")]
        public async Task<ActionResult<bool>> ValidateConfirmationCode(ValidateConfirmationCodeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error validating confirmation code!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error resetting password!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("get-masked-email")]
        public async Task<ActionResult<string>> GetMaskedEmail(string documentNumber)
        {
            try
            {
                var command = new GetMaskedEmailCommand { DocumentNumber = documentNumber };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error getting masked email!", $"{e.Message}", StatusCodes.Status400BadRequest);
                return BadRequest(problemDetails);
            }
        }
    }
}
