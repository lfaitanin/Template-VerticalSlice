using MediatR;
using WebAPI.Features.Profile.Queries;
using ProfileModel = WebAPI.Features.Profile.Models.Profile;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.Profile.Handlers
{
    public class GetProfileHandler : IRequestHandler<GetProfilesQuery, List<ProfileModel>>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetProfileHandler> _logger;
        public GetProfileHandler(IUserRepository repository, ILogger<GetProfileHandler> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<List<ProfileModel>> Handle(GetProfilesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.GetProfilesByDocumentNumberAsync(request.documentNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving profiles.", ex);
            }
        }
    }
} 