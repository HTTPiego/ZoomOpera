using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class Location : ILocation
    {
        public Guid Id { get; set; }
        public string LocationCode { get; set; }
        public string Notes { get; set; }
        public virtual Opera Opera { get; set; }
        public virtual Level Level { get; set; }
        public Guid LevelId { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is Location location &&
                   LocationCode == location.LocationCode &&
                   LevelId.Equals(location.LevelId);
        }
    }
}
