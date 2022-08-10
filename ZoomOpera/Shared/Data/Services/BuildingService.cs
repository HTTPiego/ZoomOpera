using Microsoft.EntityFrameworkCore;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Data.Services
{
    public class BuildingService : IService<IBuilding, BuildingDTO> 
    {
        private DbSet<Building> _buildings;

        private readonly ZoomOperaContext _context;

        public BuildingService(ZoomOperaContext context)
        {
            this._context = context;
            _buildings = this._context.Buildings;
        }

        public async Task<IBuilding> DeleteEntity(Guid id)
        {
            var buildingToRemove = await this._buildings.FindAsync(id);
            this._buildings.Remove(buildingToRemove);
            await this._context.SaveChangesAsync();
            return buildingToRemove;
        }

        public async Task<IEnumerable<IBuilding>> GetAll()
        {
            return await this.IncludeAllPropertiesInBuildings();
        }

        public async Task<IBuilding> AddEntity(BuildingDTO dto)
        {
            var building = new Building(dto.Name, dto.BuildingCode);

            var added = await this._context.Buildings.AddAsync(building);
            await this._context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<IBuilding> UpdateEntity(BuildingDTO updatingDto, IBuilding dbBuilding)
        {

            dbBuilding.Name = updatingDto.Name;
            dbBuilding.BuildingCode = updatingDto.BuildingCode;

            await this._context.SaveChangesAsync();
            return dbBuilding;
        }

        public async Task<IBuilding?> GetEntity(Guid entityId)
        {
            return await this._buildings.FindAsync(entityId);
        }

        private async Task<IEnumerable<IBuilding>> IncludeAllPropertiesInBuildings()
        {
            return await this._buildings.Include(building => building.Levels).ToListAsync();
        }

        public async Task<IEnumerable<IBuilding>> FindAllBy(Func<IBuilding, ValueTask<bool>> predicate)
        {
            return await this._buildings.AsAsyncEnumerable().WhereAwait(predicate).ToListAsync();
        }

        public Task<IBuilding?> FindFirstBy(Func<IBuilding, ValueTask<bool>> predicate)
        {
            return this.FindAllBy(predicate).ContinueWith(b => b.Result.FirstOrDefault());
        }

        //public async Task<Building?> Find(Guid entity)
        //{
        //    return await this._buildings.FindAsync(entity);
        //}
    }
}
