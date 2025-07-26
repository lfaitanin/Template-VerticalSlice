using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using WebAPI.Features.PreRegistration.Commands;
using UserPreRegistrationModel = WebAPI.Features.PreRegistration.Models.UserPreRegistration;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntubaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EntubaController> _logger;

        public EntubaController(IMediator mediator, ILogger<EntubaController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("import-pre-registration")]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not provided");

            var command = new ImportUserPreRegistrationsCommand { File = file };
            var result = await _mediator.Send(command);

            var list = result.Result as List<UserPreRegistrationModel>;
            if (list?.Count == 0)
                return Ok("Import completed successfully.");
            else
            {
                var problemDetails = ProblemDetailsExtensions
                    .CreateProblemDetails(HttpContext, "Error importing document numbers", $"{string.Join(",", list.Select(x => x.documentNumber))}", StatusCodes.Status404NotFound);
                return BadRequest(problemDetails);
            }
        }
    }
}
