using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Profile.Models;
using WebAPI.Features.PreRegistration.Models;
using WebAPI.Features.User.Models;
using WebAPI.Infrastructure.Data;

namespace WebAPI.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.id; 
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var exists = await _context.Users
            .Include(u => u.UserProfiles)
            .ThenInclude(up => up.Profile)
            .FirstOrDefaultAsync(u => u.email == email);

        return exists;
    }

    public async Task<User> GetUserByDocumentNumberAsync(string documentNumber)
    {
        var exists = await _context.Users
            .Include(u => u.UserProfiles)
            .ThenInclude(up => up.Profile)
            .FirstOrDefaultAsync(u => u.documentNumber == documentNumber);
        return exists;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task CompletePreRegistrationAsync(string documentNumber)
    {
        var preRegistrations = await _context.UserPreRegistrations.Where(x => x.documentNumber.Equals(documentNumber)).ToListAsync();
        if (preRegistrations is null || preRegistrations.Count == 0)
            return;

        preRegistrations.ForEach(p =>
        {
            if (!p.registrationCompleted)
                p.registrationCompleted = true;
        });

        await _context.SaveChangesAsync();
    }
    


    public async Task<User?> GetUserByEmailOrDocumentNumberAsync(string emailOrDocumentNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.email == emailOrDocumentNumber || u.documentNumber == emailOrDocumentNumber);
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<List<UserPreRegistration>> GetUserPreRegistrationAsync(string documentNumber)
    {
        var result = await _context.UserPreRegistrations.Where(x => x.documentNumber == documentNumber).ToListAsync();
        return result;
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _context.Users
                        .Include(u => u.UserProfiles)
                        .FirstOrDefaultAsync(u => u.id == id);
    }

    public async Task InsertUserProfile(UserProfile userProfile)
    {
        await _context.UserProfiles.AddAsync(userProfile);
    }

    public async Task<List<Profile>> GetProfilesByDocumentNumberAsync(string documentNumber)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.documentNumber == documentNumber);

        if (user == null)
            return new List<Profile>();

        var profiles = await _context.Profiles
            .Where(p => p.UserProfiles.Any(up => up.UserId == user.id))
            .ToListAsync();

        return profiles;
    }
    
    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }
} 