namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface ILevel
    {
        Guid Id { get; set; }
        int LevelNumber { get; set; }
        string Planimetry { get; set; }
        ICollection<Location> Locations { get; set; }
        ICollection<MonitorPlatform> MonitorPlatforms { get; set; }
        Building Building { get; set; }
        Guid BuildingId { get; set; }
        bool Equals(object? obj);
    }
}
