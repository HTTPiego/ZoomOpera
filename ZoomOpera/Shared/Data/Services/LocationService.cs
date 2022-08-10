using Microsoft.EntityFrameworkCore;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Data.Services
{
    public class LocationService : IService<ILocation, LocationDTO> //: IService<ILocation, LocationDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<Location> _locations;
        public LocationService(ZoomOperaContext context)
        {
            this._context = context;
            this._locations = this._context.Locations;
        }

        public async Task<ILocation> AddEntity(LocationDTO dto)
        {
            var location = new Location(dto.LocationCode, dto.LevelId, dto.Notes);

            var addded = await this._locations.AddAsync(location);
            await this._context.SaveChangesAsync();
            return addded.Entity;
        }

        public async Task<IEnumerable<ILocation>> FindAllBy(Func<ILocation, ValueTask<bool>> predicate)
        {
            return await this._locations.AsAsyncEnumerable().WhereAwait(predicate).ToArrayAsync();
        }

        public async Task<ILocation?> FindFirstBy(Func<ILocation, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(l => l.Result.FirstOrDefault());
        }

        public async Task<ILocation> DeleteEntity(Guid id)
        {
            var locationToRemove = await this._locations.FindAsync(id);
            this._locations.Remove(locationToRemove);
            await this._context.SaveChangesAsync();
            return locationToRemove;
        }


        public async Task<IEnumerable<ILocation>> GetAll()
        {
            return await this.IncludeAllPropertiesInLocations();
        }

        public async Task<ILocation?> GetEntity(Guid entityId)
        {
            return await this._locations.FindAsync(entityId);
        }

        public async Task<ILocation> UpdateEntity(LocationDTO updatingDTO, ILocation dbEntity)
        {
            dbEntity.LocationCode = updatingDTO.LocationCode;
            dbEntity.Notes = updatingDTO.Notes;

            await this._context.SaveChangesAsync();
            return dbEntity;
        }

        private async Task<IEnumerable<ILocation>> IncludeAllPropertiesInLocations()
        {
            return await this._locations
                            .Include(location => location.Opera)
                            .Include(location => location.Level)
                            .ToListAsync();
        }
    }
}
