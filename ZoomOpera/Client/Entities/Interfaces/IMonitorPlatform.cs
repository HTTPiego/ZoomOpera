namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IMonitorPlatform : IAccount
    {
        Guid Id { get; set; }
        string MonitorCode { get; set; }
        Level Level { get; set; }
        Guid LevelId { get; set; }
        bool Equals(object? obj);
    }
}
