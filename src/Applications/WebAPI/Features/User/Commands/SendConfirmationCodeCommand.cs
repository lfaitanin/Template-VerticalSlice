using MediatR;

namespace WebAPI.Features.User.Commands
{
    public class SendConfirmationCodeCommand : IRequest<bool>
    {
        public string DocumentNumber { get; set; }
    }
} 