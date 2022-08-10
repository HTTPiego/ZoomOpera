using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class Level : ILevel
    {
        public Guid Id { get; set; }
        public int LevelNumber { get; set; }
        public string Planimetry { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<MonitorPlatform> MonitorPlatforms { get; set; }
        public virtual Building Building { get; set; }
        public Guid BuildingId { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is Level level &&
                   LevelNumber == level.LevelNumber &&
                   BuildingId.Equals(level.BuildingId);
        }
    }
}
