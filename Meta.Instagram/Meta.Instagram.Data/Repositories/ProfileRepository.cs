using Meta.Instagram.Data.Context;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
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
    }
}
