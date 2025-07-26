using ProfileModel = WebAPI.Features.Profile.Models.Profile;
using UserPreRegistrationModel = WebAPI.Features.PreRegistration.Models.UserPreRegistration;
using WebAPI.Features.User.Models;

namespace WebAPI.Infrastructure.Repository;

public interface IEntubaRepository
{
    Task AddRangeAsync(IEnumerable<UserPreRegistrationModel> userPreRegistrations);
    Task AddAsync(UserPreRegistrationModel userPreRegistration);
    Task AddProfileAsync(ProfileModel profile);
    Task<bool> Exists(string documentNumber);
    Task<ValidaCadastroDto> ValidaDocumentNumber(string documentNumber);
}
