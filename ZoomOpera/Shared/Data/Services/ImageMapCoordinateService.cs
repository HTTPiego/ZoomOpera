using Microsoft.EntityFrameworkCore;
using ZoomOpera.DTOs;
using ZoomOpera.Server.Data;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Data.Services
{
    public class ImageMapCoordinateService : IService<IImageMapCoordinate, ImageMapCoordinateDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<ImageMapCoordinate> _coordinates;

        public ImageMapCoordinateService(ZoomOperaContext context)
        {
            _context = context;
            _coordinates = _context.ImageMapCoordinates;
        }

        public async Task<IImageMapCoordinate> AddEntity(ImageMapCoordinateDTO dto)
        {
            var imageMapCoordinate = new ImageMapCoordinate(dto.X, dto.Y, dto.Position, dto.ImageMapId);

            var added = await this._coordinates.AddAsync(imageMapCoordinate);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IImageMapCoordinate> DeleteEntity(Guid id)
        {
            var coordinateToRemove = await this._coordinates.FindAsync(id);
            this._coordinates.Remove(coordinateToRemove);
            await this._context.SaveChangesAsync();
            return coordinateToRemove;
        }

        public async Task<IEnumerable<IImageMapCoordinate>> FindAllBy(Func<IImageMapCoordinate, ValueTask<bool>> predicate)
        {
            return await this._coordinates.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<IImageMapCoordinate?> FindFirstBy(Func<IImageMapCoordinate, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(o => o.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<IImageMapCoordinate>> GetAll()
        {
            return await this.IncludeAllPropertiesInCoordinate();
        }

        public async Task<IImageMapCoordinate?> GetEntity(Guid entityId)
        {
            return await this._coordinates.FindAsync(entityId);
        }

        public async Task<IImageMapCoordinate> UpdateEntity(ImageMapCoordinateDTO updatingDTO, IImageMapCoordinate dbEntity)
        {
            dbEntity.X = updatingDTO.X;
            dbEntity.Y = updatingDTO.Y;
            dbEntity.Position = updatingDTO.Position;

            await this._context.SaveChangesAsync();

            return dbEntity;
        }

        private async Task<IEnumerable<IImageMapCoordinate>> IncludeAllPropertiesInCoordinate()
        {
            return await this._coordinates.Include(coordinate => coordinate.ImageMap).ToListAsync();
        }

    }
}
