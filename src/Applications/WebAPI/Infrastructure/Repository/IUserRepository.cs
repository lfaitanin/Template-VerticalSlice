using WebAPI.Features.Profile.Models;
using WebAPI.Features.PreRegistration.Models;
using WebAPI.Features.User.Models;

namespace WebAPI.Infrastructure.Repository;

public interface IUserRepository
{
    public Task<Guid> AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByDocumentNumberAsync(string documentNumber);
    Task<List<User>> GetAllUsersAsync();
    Task CompletePreRegistrationAsync(string documentNumber);
    Task<User?> GetUserByEmailOrDocumentNumberAsync(string emailOrDocumentNumber);
    Task UpdateUserAsync(User user);
    Task CommitAsync();

    Task<List<UserPreRegistration>> GetUserPreRegistrationAsync(string documentNumber);
    Task<List<Profile>> GetProfilesByDocumentNumberAsync(string documentNumber);
    Task<User> GetUserByIdAsync(Guid id);
    Task InsertUserProfile(UserProfile userProfile);
} 