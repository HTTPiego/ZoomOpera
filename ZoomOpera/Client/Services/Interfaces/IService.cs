namespace ZoomOpera.Client.Services.Interfaces
{
    public interface IService<TEntity, DTO>
    {
        //Task<IEnumerable<TEntity>> FindAllBy(Func<TEntity, ValueTask<bool>> predicate);
        //Task<TEntity?> FindFirstBy(Func<TEntity, ValueTask<bool>> predicate);

        Task<IEnumerable<TEntity>?> GetAll();
        Task<IEnumerable<TEntity>?> GetAllByfatherRelationshipId(Guid fatherId);
        Task<TEntity?> GetEntityByfatherRelationshipId(Guid fatherId);
        Task<TEntity?> GetEntity(Guid entityId);
        Task<TEntity?> AddEntity(DTO dto);
        Task<TEntity?> DeleteEntity(Guid id);   
        Task<TEntity?> UpdateEntity(DTO updatingDTO, Guid idEntity);
    }
}
