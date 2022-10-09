using Microsoft.EntityFrameworkCore;
using ZoomOpera.DTOs;
using ZoomOpera.Server.Data;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Data.Services
{
    public class OperaImageService : IService<IOperaImage, OperaImageDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<OperaImage> _operaImages;

        public OperaImageService(ZoomOperaContext context)
        {
            _context = context;
            _operaImages = _context.OperaImages;
        }

        public async Task<IOperaImage> AddEntity(OperaImageDTO dto)
        {
            var operaImage = new OperaImage(dto.Image, dto.Height, dto.Width, dto.OperaId);

            var added = await _operaImages.AddAsync(operaImage);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IOperaImage> DeleteEntity(Guid id)
        {
            var operaImageToRemove = await _operaImages.FindAsync(id);
            _operaImages.Remove(operaImageToRemove);
            await _context.SaveChangesAsync();
            return operaImageToRemove;
        }

        public async Task<IEnumerable<IOperaImage>> FindAllBy(Func<IOperaImage, ValueTask<bool>> predicate)
        {
            return await this._operaImages.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<IOperaImage?> FindFirstBy(Func<IOperaImage, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(o => o.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<IOperaImage>> GetAll()
        {
            return await this.IncludeAllPropertiesInOperaImages();
        }

        public async Task<IOperaImage?> GetEntity(Guid entityId)
        {
            return await this._operaImages.FindAsync(entityId);
        }

        public async Task<IOperaImage> UpdateEntity(OperaImageDTO updatingDTO, IOperaImage dbEntity)
        {
            dbEntity.Image = updatingDTO.Image;
            dbEntity.Width = updatingDTO.Width;
            dbEntity.Height = updatingDTO.Height;

            await _context.SaveChangesAsync();

            return dbEntity;
        }

        private async Task<IEnumerable<IOperaImage>> IncludeAllPropertiesInOperaImages()
        {
            return await this._operaImages.Include(operaImage => operaImage.Opera)
                                            .Include(operaImage => operaImage.ImageMaps)
                                            .ToListAsync();
        }
    }
}
