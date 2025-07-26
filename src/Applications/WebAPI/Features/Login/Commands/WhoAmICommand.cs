using MediatR;
using WebAPI.Features.Login.Models;

namespace WebAPI.Features.Login.Commands
{
    public class WhoAmICommand : IRequest<WhoAmIResponse>
    {
        public Guid UserId { get; set; }
    }
}
