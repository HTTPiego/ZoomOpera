using Microsoft.EntityFrameworkCore;
using ZoomOpera.DTOs;
using ZoomOpera.Server.Data;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Data.Services
{
    public class ImageMapService : IService<IImageMap, ImageMapDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<ImageMap> _imageMaps;

        public ImageMapService(ZoomOperaContext context)
        {
            _context = context;
            _imageMaps = _context.ImageMaps;
        }

        public async Task<IImageMap> AddEntity(ImageMapDTO dto)
        {
            var imageMap = new ImageMap(dto.ImageMapShape,
                                        dto.Title,
                                        dto.DetailedDescription,
                                        dto.OperaImageId);

            var added = await this._imageMaps.AddAsync(imageMap);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IImageMap> DeleteEntity(Guid id)
        {
            var imageMapToRemove = await this._imageMaps.FindAsync(id);
            this._imageMaps.Remove(imageMapToRemove);
            await this._context.SaveChangesAsync();
            return imageMapToRemove;
        }

        public async Task<IEnumerable<IImageMap>> FindAllBy(Func<IImageMap, ValueTask<bool>> predicate)
        {
            return await this._imageMaps.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<IImageMap?> FindFirstBy(Func<IImageMap, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(o => o.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<IImageMap>> GetAll()
        {
            return await this.IncludeAllPropertiesInImageMaps();
        }

        public async Task<IImageMap?> GetEntity(Guid entityId)
        {
            return await this._imageMaps.FindAsync(entityId);
        }

        public Task<IImageMap> UpdateEntity(ImageMapDTO updatingDTO, IImageMap dbEntity)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<IImageMap>> IncludeAllPropertiesInImageMaps()
        {
            return await this._imageMaps.Include(imageMap => imageMap.OperaImage)
                                        .Include(imageMap => imageMap.ImageMapCoordinates)
                                        .ToListAsync();
        }
    }
}
