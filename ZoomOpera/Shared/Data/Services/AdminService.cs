using Microsoft.EntityFrameworkCore;
using ZoomOpera.Server.Data;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Data.Services
{
    public class AdminService : IService<IAdmin, AdminDTO> //: IService<IAdmin, AdminDTO> 
    {
        private readonly ZoomOperaContext _context;

        private DbSet<Admin> _admins;
        public AdminService(ZoomOperaContext context)
        {
            this._context = context;
            this._admins = this._context.Admins;
        }
        public async Task<IAdmin> AddEntity(AdminDTO dto)
        {
            var admin = new Admin(dto.Name, 
                                  dto.Password, 
                                  dto.Email, 
                                  dto.Surname, 
                                  dto.GivenName);

            var added = await this._admins.AddAsync(admin);
            await this._context.SaveChangesAsync();
            return added.Entity;
            //return admin;
        }

        public async Task<IEnumerable<IAdmin>> FindAllBy(Func<IAdmin, ValueTask<bool>> predicate)
        {
            return await this._admins.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }
        
        public async Task<IAdmin?> FindFirstBy(Func<IAdmin, ValueTask<bool>> predicate)
        {
            return await this.FindAllBy(predicate).ContinueWith(a => a.Result.FirstOrDefault());
        }

        public async Task<IAdmin> DeleteEntity(Guid id)
        {
            var adminToRemove = await this._admins.FindAsync(id);
            this._admins.Remove(adminToRemove);
            await this._context.SaveChangesAsync();
            return adminToRemove;
        }


        public async Task<IEnumerable<IAdmin>> GetAll()
        {
            return await this._admins.ToListAsync();
        }

        public async Task<IAdmin?> GetEntity(Guid adminId)
        {
            return await this._admins.FindAsync(adminId);
        }

        public async Task<IAdmin> UpdateEntity(AdminDTO updatingDto, IAdmin dbAdmin)
        {
            dbAdmin.Name = updatingDto.Name;
            dbAdmin.Password = updatingDto.Password;
            dbAdmin.GivenName = updatingDto.GivenName;
            dbAdmin.Surname = updatingDto.Surname;
            dbAdmin.Email = updatingDto.Email;

            await this._context.SaveChangesAsync();
            return dbAdmin;
        }

    }
}
