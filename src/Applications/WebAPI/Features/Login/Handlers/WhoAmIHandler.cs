using MediatR;
using WebAPI.Features.Login.Commands;
using WebAPI.Features.Login.Models;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.Login.Handlers
{
    public class WhoAmIHandler : IRequestHandler<WhoAmICommand, WhoAmIResponse>
    {
        private readonly IUserRepository _repository;
        public WhoAmIHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<WhoAmIResponse> Handle(WhoAmICommand request, CancellationToken cancellationToken)
        {
            // Get user from repository by ID
            var user = await _repository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Return user data
            return new WhoAmIResponse
            {
                UserId = user.id,
                Name = user.name,
                Email = user.email,
                DocumentNumber = user.documentNumber,
                Profiles = user.UserProfiles.Select(x => x.ProfileId),
                Blocked = user.blocked,
                Photo = user.photo,
                Phone = user.phone
            };
        }
    }
}
