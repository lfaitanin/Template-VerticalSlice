using MediatR;

namespace WebAPI.Features.User.Commands
{
    public class ValidateConfirmationCodeCommand : IRequest<bool>
    {
        public string DocumentNumber { get; set; }
        public string Code { get; set; }
    }
} 