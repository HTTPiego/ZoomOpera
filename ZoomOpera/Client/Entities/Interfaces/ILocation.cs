namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface ILocation
    {
        Guid Id { get; set; }
        string LocationCode { get; set; }
        string Notes { get; set; }
        Opera Opera { get; set; }
        Level Level { get; set; }
        Guid LevelId { get; set; }
        bool Equals(object? obj);
    }
}
