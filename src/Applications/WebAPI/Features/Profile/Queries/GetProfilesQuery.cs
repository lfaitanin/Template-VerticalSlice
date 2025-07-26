using MediatR;
using ProfileModel = WebAPI.Features.Profile.Models.Profile;

namespace WebAPI.Features.Profile.Queries
{
    public class GetProfilesQuery : IRequest<List<ProfileModel>>
    {
        public string documentNumber { get; set; }
    }
} 