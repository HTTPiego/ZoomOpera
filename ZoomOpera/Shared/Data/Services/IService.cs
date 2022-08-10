
namespace ZoomOpera.Shared.Data.Services
{
    public interface IService<IEntity, TDto>
    {
        Task<IEnumerable<IEntity>> FindAllBy(Func<IEntity, ValueTask<bool>> predicate);
        Task<IEntity?> FindFirstBy(Func<IEntity, ValueTask<bool>> predicate);
        Task<IEnumerable<IEntity>> GetAll();
        Task<IEntity?> GetEntity(Guid entityId);
        Task<IEntity> AddEntity(TDto dto);
        Task<IEntity> DeleteEntity(Guid id);
        Task<IEntity> UpdateEntity(TDto updatingDTO, IEntity dbEntity);

        //List<TEntity> IncludeAllProperties(DbSet<TEntity> entities);
    }
}
