using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class MonitorPlatform : IMonitorPlatform
    {
        public Guid Id { get; set; }
        public string MonitorCode { get; set; }
        public virtual Level Level { get; set; }
        public Guid LevelId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; init; } = "MonitorPlatform";
        public override bool Equals(object? obj)
        {
            return obj is MonitorPlatform platform &&
                   MonitorCode == platform.MonitorCode &&
                   LevelId.Equals(platform.LevelId) &&
                   Name == platform.Name;
        }
    }
}
