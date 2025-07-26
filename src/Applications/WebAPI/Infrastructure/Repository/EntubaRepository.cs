using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities.Enum;
using ProfileModel = WebAPI.Features.Profile.Models.Profile;
using UserPreRegistrationModel = WebAPI.Features.PreRegistration.Models.UserPreRegistration;
using WebAPI.Features.User.Models;
using WebAPI.Infrastructure.Data;

namespace WebAPI.Infrastructure.Repository
{
    public class EntubaRepository : IEntubaRepository
    {
        private readonly ApplicationDbContext _context;

        public EntubaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<UserPreRegistrationModel> userPreRegistrations)
        {
            try
            {
                await _context.UserPreRegistrations.AddRangeAsync(userPreRegistrations);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {

            }
        }

        public async Task AddAsync(UserPreRegistrationModel userPreRegistration)
        {
            await _context.UserPreRegistrations.AddAsync(userPreRegistration);
            await _context.SaveChangesAsync();
        }

        public async Task AddProfileAsync(ProfileModel profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string documentNumber)
        {
            return await _context.UserPreRegistrations.Where(x => x.documentNumber == documentNumber).FirstOrDefaultAsync() != null;
        }

        public async Task<ValidaCadastroDto> ValidaDocumentNumber(string documentNumber)
        {
            var response = new ValidaCadastroDto();
            var result = await _context.UserPreRegistrations.Where(x => x.documentNumber == documentNumber).ToListAsync();

            if (result is null || result.Count == 0)
                return default;

            response.profileIds = result.Select(x => (int)x.profileId).ToList();
            response.name = result.FirstOrDefault().name;
            response.registrationCompleted = result.FirstOrDefault().registrationCompleted;
            
            return response;
        }
    }
}
