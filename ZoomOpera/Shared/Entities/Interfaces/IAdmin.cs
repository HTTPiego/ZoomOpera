using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IAdmin : IAccount, IUser
    {
        Guid Id { get; set; }
        bool Equals(object? obj);
        bool Equals(AdminDTO dto);
    }
}
