using MediatR;

namespace WebAPI.Features.Profile.Commands
{
    public class CreateProfileCommand : IRequest<int>
    {
        public string description { get; set; }
        public string status { get; set; }
    }
} 