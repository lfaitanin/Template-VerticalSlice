using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Entities.Response;

namespace WebAPI.Features.PreRegistration.Commands
{
    public class ImportUserPreRegistrationsCommand : IRequest<BaseResponse>
    {
        public IFormFile File { get; set; }
    }
} 