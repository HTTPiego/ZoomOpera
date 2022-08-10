using Microsoft.EntityFrameworkCore;
using ZoomOpera.Server.Data;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Data.Services
{
    public class MonitorPlatformService : IService<IMonitorPlatform, MonitorPlatformDTO> //: IService<IMonitorPlatform, MonitorPlatformDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<MonitorPlatform> _platforms;
        public MonitorPlatformService(ZoomOperaContext context)
        {
            this._context = context;
            this._platforms = this._context.MonitorPlatforms;
        }
        public async Task<IMonitorPlatform> AddEntity(MonitorPlatformDTO dto)
        {
            var monitorPlatform = new MonitorPlatform(dto.MonitorCode,
                                                      dto.LevelId,
                                                      dto.Name,
                                                      dto.Password);

            var added = await this._platforms.AddAsync(monitorPlatform);
            await this._context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IEnumerable<IMonitorPlatform>> FindAllBy(Func<IMonitorPlatform, ValueTask<bool>> predicate)
        {
            return await this._platforms.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<IMonitorPlatform?> FindFirstBy(Func<IMonitorPlatform, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(p => p.Result.FirstOrDefault());
        }

        public async Task<IMonitorPlatform> DeleteEntity(Guid id)
        {
            var platformToRemove = await this._platforms.FindAsync(id);
            this._platforms.Remove(platformToRemove);
            await this._context.SaveChangesAsync();
            return platformToRemove;
        }

        public async Task<IEnumerable<IMonitorPlatform>> GetAll()
        {
            return await this.IncludeAllPropertiesInPlatforms();
        }

        public async Task<IMonitorPlatform?> GetEntity(Guid entityId)
        {
            return await this._platforms.FindAsync(entityId);
        }

        public async Task<IMonitorPlatform> UpdateEntity(MonitorPlatformDTO updatingDto, IMonitorPlatform dbMonitorPlatform)
        {
            dbMonitorPlatform.MonitorCode = updatingDto.MonitorCode;
            dbMonitorPlatform.Name = updatingDto.Name;
            dbMonitorPlatform.Password = updatingDto.Password;

            await this._context.SaveChangesAsync();
            return dbMonitorPlatform;
        }

        private async Task<IEnumerable<IMonitorPlatform>> IncludeAllPropertiesInPlatforms()
        {
            return await this._platforms.Include(platform => platform.Level).ToListAsync();
        }

    }
}
