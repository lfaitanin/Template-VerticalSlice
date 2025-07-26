using MediatR;

namespace WebAPI.Features.User.Commands
{
    public class GetMaskedEmailCommand : IRequest<string>
    {
        public string DocumentNumber { get; set; }
    }
} 