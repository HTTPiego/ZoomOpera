using Microsoft.EntityFrameworkCore;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Data.Services
{
    public class OperaService : IService<IOpera, OperaDTO>
    {
        private readonly ZoomOperaContext _context;

        private DbSet<Opera> _operas;
        public OperaService(ZoomOperaContext context)
        {
            _context = context;
            _operas = this._context.Operas;
        }

        public async Task<IOpera> AddEntity(OperaDTO dto)
        {
            var opera = new Opera(dto.Name,
                                    dto.ItalianDescription,
                                    dto.AuthorFirstName,
                                    dto.AuthorLastName,
                                    //dto.Photo,
                                    dto.LocationId);

            var added = await this._operas.AddAsync(opera);
            await this._context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IOpera> DeleteEntity(Guid id)
        {
            var operaToRemove = await this._operas.FindAsync(id);
            this._operas.Remove(operaToRemove);
            await this._context.SaveChangesAsync();
            return operaToRemove;
        }

        public async Task<IEnumerable<IOpera>> FindAllBy(Func<IOpera, ValueTask<bool>> predicate)
        {
            return await this._operas.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public async Task<IOpera?> FindFirstBy(Func<IOpera, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(o => o.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<IOpera>> GetAll()
        {
            return await this.IncludeAllPropertiesInOperas();
        }

        public async Task<IOpera?> GetEntity(Guid entityId)
        {
            return await this._operas.FindAsync(entityId);
        }

        public async Task<IOpera> UpdateEntity(OperaDTO updatingDTO, IOpera dbOpera)
        {
            dbOpera.Name = updatingDTO.Name;
            dbOpera.ItalianDescription = updatingDTO.ItalianDescription;
            //dbOpera.Photo = updatingDTO.Photo;

            await this._context.SaveChangesAsync();

            return dbOpera;
        }

        private async Task<IEnumerable<IOpera>> IncludeAllPropertiesInOperas()
        {
            return await this._operas.Include(opera => opera.Location)
                                        .Include(opera => opera.Image)
                                        .ToListAsync();
        }
    }
}
