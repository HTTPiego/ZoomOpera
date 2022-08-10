using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IMonitorPlatform : IAccount
    {
        Guid Id { get; set; }
        string MonitorCode { get; set; }
        Level Level { get; set; }
        Guid LevelId { get; set; }
        public bool Equals(object? obj);
        public bool EqualsTo(MonitorPlatformDTO dto);
    }
}
