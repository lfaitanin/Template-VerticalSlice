using MediatR;

namespace WebAPI.Features.User.Commands
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string DocumentNumber { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
} 