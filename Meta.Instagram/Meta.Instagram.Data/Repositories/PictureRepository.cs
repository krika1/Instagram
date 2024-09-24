using Meta.Instagram.Data.Context;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Meta.Instagram.Data.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PictureRepository> _logger;

        public PictureRepository(ApplicationDbContext db, ILogger<PictureRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task DeletePictureAsync(Picture picture)
        {
            try
            {
                _db.Pictures.Remove(picture);

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<Picture> GetPictureAsync(string pictureId)
        {
            try
            {
                var picture = await _db.Pictures.FirstOrDefaultAsync(x => x.PictureId == pictureId);

                return picture!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new DatabaseException(ex.Message);
            }
        }
    }
}
