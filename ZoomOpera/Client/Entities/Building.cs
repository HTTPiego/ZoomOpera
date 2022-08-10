using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class Building : IBuilding
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BuildingCode { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is Building building &&
                   Name == building.Name &&
                   BuildingCode == building.BuildingCode;
        }
    }
}
