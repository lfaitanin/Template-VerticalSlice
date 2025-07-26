using MediatR;
using WebAPI.Features.Profile.Commands;
using ProfileModel = WebAPI.Features.Profile.Models.Profile;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.Profile.Handlers
{
    public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, int>
    {
        private readonly IEntubaRepository _repository;

        public CreateProfileHandler(IEntubaRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new ProfileModel
            {
                description = request.description,
                status = request.status
            };

            await _repository.AddProfileAsync(profile);
            return profile.id;
        }
    }
} 