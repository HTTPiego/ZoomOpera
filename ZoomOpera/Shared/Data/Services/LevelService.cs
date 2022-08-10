using Microsoft.EntityFrameworkCore;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Data.Services
{
    public class LevelService : IService<ILevel, LevelDTO> //: IService<ILevel, LevelDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<Level> _levels;
        public LevelService(ZoomOperaContext context)
        {
            _context = context;
            this._levels = this._context.Levels;
        }

        public async Task<ILevel> AddEntity(LevelDTO dto)
        {
            var level = new Level(dto.LevelNumber, dto.Planimetry, dto.BuildingId);

            var added = await this._levels.AddAsync(level);
            await this._context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IEnumerable<ILevel>> FindAllBy(Func<ILevel, ValueTask<bool>> predicate)
        {
            return await this._levels.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<ILevel?> FindFirstBy(Func<ILevel, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(l => l.Result.FirstOrDefault());
        }

        public async Task<ILevel> DeleteEntity(Guid id)
        {
            var levelToRemove = await this._levels.FindAsync(id);
            this._levels.Remove(levelToRemove);
            await this._context.SaveChangesAsync();
            return levelToRemove;
        }


        public async Task<IEnumerable<ILevel>> GetAll()
        {
            return await this.IncludeAllPropertiesInLevels();
        }

        public async Task<ILevel?> GetEntity(Guid entityId)
        {
            return await this._levels.FindAsync(entityId);
        }
        public async Task<ILevel> UpdateEntity(LevelDTO updatingDto, ILevel dbLevel)
        {

            dbLevel.LevelNumber = updatingDto.LevelNumber;
            dbLevel.Planimetry = updatingDto.Planimetry;

            await this._context.SaveChangesAsync();

            return dbLevel;
        }

        private async Task<IEnumerable<ILevel>> IncludeAllPropertiesInLevels()
        {
            return await this._levels.Include(level => level.Building)
                               .Include(level => level.MonitorPlatforms)
                               .Include(level => level.Locations)
                               .ToListAsync();
        }
    }
}
