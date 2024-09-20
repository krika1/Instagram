using Meta.Instagram.Data.Context;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Meta.Instagram.Data.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AccountRepository> _logger;

        public ProfileRepository(ApplicationDbContext db, ILogger<AccountRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task CreateProfileAsync(Profile profile)
        {
            try
            {
                var entry = await _db.Profiles.AddAsync(profile);

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<Profile> GetProfileAsync(string profileId)
        {

            try
            {
                var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.ProfileId == profileId);

                return profile!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<Profile> UpdateProfileAsync(Profile profile)
        {
            try
            {
                var updatedEntry = _db.Profiles.Update(profile);

                await _db.SaveChangesAsync();

                return updatedEntry.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new DatabaseException(ex.Message);
            }
        }
    }
}
